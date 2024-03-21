using APIBanco.Domain.Models;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using APIBanco.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace APIBanco.Services;

public class ClientService
{
    private readonly AppDbContext _dbContext;

    public ClientService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> LoginAsync(ClientLoginRequestDto client)
    {
        JwtService _jwtService = new JwtService();

        Client? user = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Cpf == client.Cpf && x.Password == client.Password).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException("Cpf or Password not correct.");
        }

        string? token = _jwtService.GenerateToken(user);

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
        EntityEntry<Client>? res = await _dbContext.Clients.AddAsync(Client);
        await _dbContext.SaveChangesAsync();

        return Client;
    }

    public async Task<Client> UpdateAsync(Client Client, int Id)
    {
        Client? oldClient = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Id == Id).FirstOrDefaultAsync();

        if (oldClient == null)
            throw new KeyNotFoundException("Client not found: " + Id);

        oldClient.UpdatedAt = DateTime.Now;
        oldClient.Name = Client.Name;
        oldClient.Email = Client.Email;
        oldClient.Password = Client.Password;
        oldClient.PhoneNumber = Client.PhoneNumber;
        oldClient.BornDate = Client.BornDate;
        // oldClient.Adress = client.Adress;

        _dbContext.Clients.Update(oldClient);
        await _dbContext.SaveChangesAsync();

        return oldClient;
    }
}
