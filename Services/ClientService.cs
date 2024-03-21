using APIBanco.Domain.Models;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace APIBanco.Services;

public class ClientService
{
    private readonly AppDbContext _dbContext;

    public ClientService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Client>> GetAsync()
    {
        return await _dbContext.Clients.ToListAsync();
    }

    public async Task<Client> GetByIdAsync(int id)
    {
        Client? response = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Id == id).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Client not found: " + id);

        return response;
    }

    public async Task<Client> GetByCpfAsync(ulong cpf)
    {
        Client? response = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Cpf == cpf).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Client not found: " + cpf);

        return response;
    }

    public async Task<Client> CreateAsync(Client client)
    {
        client.BankAccount = new BankAccount();
        client.BankAccount.Cpf = client.Cpf;
        EntityEntry<Client>? res = await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
        // catch (DbUpdateException ex)
        //     {

        //         if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
        //         {

        //             Console.WriteLine("Conflito de chave primária detectado.");
        //         }
        //     }

        return client;
    }

    public async Task<Client> UpdateAsync(Client client, int id)
    {
        Client? oldClient = await _dbContext.Clients.AsQueryable().Where(predicate: x => x.Id == id).FirstOrDefaultAsync();

        if (oldClient == null)
            throw new KeyNotFoundException("Client not found: " + id);

        oldClient.UpdatedAt = DateTime.Now;
        oldClient.Name = client.Name;
        oldClient.Email = client.Email;
        oldClient.Password = client.Password;
        oldClient.PhoneNumber = client.PhoneNumber;
        oldClient.BornDate = client.BornDate;
        // oldClient.Adress = client.Adress;

        _dbContext.Clients.Update(oldClient);
        await _dbContext.SaveChangesAsync();

        return oldClient;
    }
}
