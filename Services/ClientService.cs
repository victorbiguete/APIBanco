using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using APIBanco.Domain.Dtos;
using APIBanco.Domain.Models.DbContext;

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

    /// <summary>
    /// Authenticates a client using its Cpf and Password.
    /// </summary>
    /// <param name="client">A DTO with Cpf and Password of the client.</param>
    /// <returns>A JWT token if the authentication is successful.</returns>
    /// <exception cref="KeyNotFoundException">If the Cpf or Password is incorrect.</exception>
    public async Task<string> LoginAsync(ClientLoginRequestDto client)
    {
        Client? clientLogin = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Cpf == client.Cpf).FirstOrDefaultAsync();

        if (clientLogin == null)
        {
            throw new KeyNotFoundException("Cpf   or Password incorrect.");
        }

        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(client.Password, clientLogin.Password);

        if (!isPasswordCorrect)
        {
            throw new KeyNotFoundException("Cpf   or Password incorrect.");
        }

        clientLogin.BankAccount.StatusCheck();

        string token = _jwtService.GenerateToken(client: clientLogin);

        return token;
    }

    /// <summary>
    /// Retrieves all clients from the database.
    /// </summary>
    /// <returns>A collection of <see cref="Client"/> objects.</returns>
    public async Task<IEnumerable<Client>> GetAsync()
    {
        return await _dbContext.Clients.ToListAsync();
    }

    /// <summary>
    /// Retrieves a client from the database by its Id.
    /// </summary>
    /// <param name="Id">The Id of the client.</param>
    /// <returns>The client with the given Id.</returns>
    /// <exception cref="KeyNotFoundException">If the client with the given Id is not found.</exception>
    public async Task<Client> GetByIdAsync(int Id)
    {
        Client? response = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Id == Id).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Client not found: " + Id);

        return response;
    }

    /// <summary>
    /// Retrieves a client from the database by its CPF.
    /// </summary>
    /// <param name="Cpf">The CPF of the client.</param>
    /// <returns>The client with the given CPF.</returns>
    /// <exception cref="KeyNotFoundException">If the client with the given CPF is not found.</exception>
    public async Task<Client> GetByCpfAsync(string Cpf)
    {
        Client? response = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Cpf == Cpf).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Client not found: " + Cpf);

        return response;
    }

    /// <summary>
    /// Creates a new client in the database.
    /// </summary>
    /// <param name="Client">The client to be created.</param>
    /// <returns>The created client.</returns>
    public async Task<Client> CreateAsync(Client Client)
    {
        Client.ValidateCpf();

        Client.BankAccount = new BankAccount();
        Client.BankAccount.Cpf = Client.Cpf;

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(Client.Password);
        Client.Password = hashPassword;

        EntityEntry<Client>? res = await _dbContext.Clients.AddAsync(Client);
        await _dbContext.SaveChangesAsync();

        return Client;
    }

    /// <summary>
    /// Updates a client in the database.
    /// </summary>
    /// <param name="Client">The client with the new information.</param>
    /// <param name="Id">The id of the client to be updated.</param>
    /// <returns>The updated client.</returns>
    /// <exception cref="KeyNotFoundException">If the client with the given Id is not found.</exception>
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

        _dbContext.Clients.Update(oldClient);
        await _dbContext.SaveChangesAsync();

        return oldClient;
    }
}
