using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

using APIBanco.Domain.Models;
using APIBanco.Domain.Enums;

namespace APIBanco.Services;

public class BankAccountService
{
    private readonly IMongoCollection<BankAccount> _bankAccount;

    public BankAccountService(IOptions<MongoDBSettings> settings)
    {
        MongoClient? client = new MongoClient(connectionString: settings.Value.ConnectionURI);
        IMongoDatabase? database = client.GetDatabase(name: settings.Value.DatabaseName);
        _bankAccount = database.GetCollection<BankAccount>(name: settings.Value.CollectionBankAccount);
    }

    public async Task<IEnumerable<BankAccount>> GetAsync()
    {
        return await _bankAccount.Find(filter: _ => true).ToListAsync();
    }

    public async Task<BankAccount> GetAsync(string Id)
    {
        FilterDefinition<BankAccount>? filter = Builders<BankAccount>.Filter.Eq(field: "_id", value: ObjectId.Parse(s: Id));
        BankAccount? response = await _bankAccount.Find(filter: filter).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("BankAccount not found: " + Id);

        return response;
    }

    public async Task<IEnumerable<BankAccount>> GetAsync(int Cpf)
    {
        FilterDefinition<BankAccount>? filter = Builders<BankAccount>.Filter.Eq(field: x => x.Cpf, Cpf);
        List<BankAccount>? response = await _bankAccount.Find(filter: filter).ToListAsync();

        if (response.Count == 0)
            throw new KeyNotFoundException("BankAccount not found: " + Cpf);

        return response;
    }

    public async Task<BankAccount> CreateAsync(BankAccount bankAccount)
    {
        await _bankAccount.InsertOneAsync(document: bankAccount);
        return bankAccount;
    }

    public async Task<BankAccount> UpdateAsync(BankAccount bankAccount)
    {
        FilterDefinition<BankAccount>? filter = Builders<BankAccount>.Filter.Eq(field: x => x.Cpf, value: bankAccount.Cpf);
        BankAccount? oldBankAccount = await _bankAccount.Find(filter: filter).FirstOrDefaultAsync();

        bankAccount.Id = oldBankAccount.Id;
        bankAccount.UpdatedAt = DateTime.Now;

        await _bankAccount.ReplaceOneAsync(filter: filter, replacement: bankAccount);

        return bankAccount;
    }

    public async Task<BankAccount> UpdateStatusAsync(int Cpf, AccountStatus status)
    {
        FilterDefinition<BankAccount>? filter = Builders<BankAccount>.Filter.Eq(field: x => x.Cpf, value: Cpf);
        BankAccount? oldBankAccount = await _bankAccount.Find(filter: filter).FirstOrDefaultAsync();

        if (oldBankAccount == null)
            throw new KeyNotFoundException("BankAccount not found: " + Cpf);

        oldBankAccount.Status = status;
        oldBankAccount.UpdatedAt = DateTime.Now;

        await _bankAccount.ReplaceOneAsync(filter: filter, replacement: oldBankAccount);

        return oldBankAccount;
    }

    public async Task<long> Length()
    {
        long lenght = await _bankAccount.CountDocumentsAsync(filter: new BsonDocument());
        return lenght;
    }
}
