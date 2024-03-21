using APIBanco.Domain.Models;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using APIBanco.Domain.Dtos;

namespace APIBanco.Services;

public class ClientService
{
    private readonly AppDbContext _dbContext;
    private readonly JwtService _jwtService;

    public ClientService(AppDbContext dbContext, JwtService jwtService)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
    }

    public async Task<string> LoginAsync(ClientLoginRequestDto client)
    {
        Client? clientLogin = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Cpf == client.Cpf).FirstOrDefaultAsync();

        if (clientLogin == null)
        {
            throw new KeyNotFoundException("Cpf or Password incorrect.");
        }

        string? passwordHash = BCrypt.Net.BCrypt.HashPassword(client.Password);
        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(client.Password, passwordHash);

        if (!isPasswordCorrect)
        {
            throw new KeyNotFoundException("Cpf or Password incorrect.");
        }

        string token = _jwtService.GenerateToken(client: clientLogin);

        return token;
    }

    public async Task<List<Client>> GetAsync()
    {
        return await _dbContext.Clients.ToListAsync();
    }

    public async Task<Client> GetByIdAsync(int Id)
    {
        Client? response = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Id == Id).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Client not found: " + Id);

        return response;
    }

    public async Task<Client> GetByCpfAsync(ulong Cpf)
    {
        Client? response = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Cpf == Cpf).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Client not found: " + Cpf);

        return response;
    }

    public async Task<Client> CreateAsync(Client Client)
    {
        Client.BankAccount = new BankAccount();
        Client.BankAccount.Cpf = Client.Cpf;

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(Client.Password);
        Client.Password = hashPassword;

        EntityEntry<Client>? res = await _dbContext.Clients.AddAsync(Client);
        await _dbContext.SaveChangesAsync();

        return Client;
    }

    public async Task<Client> UpdateAsync(Client Client, int Id)
    {
        Client? oldClient = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Id == Id).FirstOrDefaultAsync();

        if (oldClient == null)
            throw new KeyNotFoundException("Client not found: " + Id);

        oldClient.UpdatedAt = DateTime.UtcNow;
        oldClient.Name = Client.Name;
        oldClient.Email = Client.Email;

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(Client.Password);
        oldClient.Password = hashPassword;

        oldClient.PhoneNumber = Client.PhoneNumber;
        oldClient.BornDate = Client.BornDate;
        // oldClient.Adress = client.Adress;

        _dbContext.Clients.Update(oldClient);
        await _dbContext.SaveChangesAsync();

        return oldClient;
    }
}
