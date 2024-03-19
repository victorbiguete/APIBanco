using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

using APIBanco.Domain.Models;

namespace APIBanco.Services;

public class ClientService
{
    private IMongoCollection<Client> _clients;

    private BankAccountService _bankAccountService;
    private AdressService _adressService;

    public ClientService(IOptions<MongoDBSettings> settings, BankAccountService bankAccountService, AdressService adressService)
    {
        MongoClient? client = new MongoClient(connectionString: settings.Value.ConnectionURI);
        IMongoDatabase? database = client.GetDatabase(name: settings.Value.DatabaseName);
        _clients = database.GetCollection<Client>(name: settings.Value.CollectionClient);

        // make  cpf be unique
        IndexKeysDefinition<Client>? indexKeysDefinition = Builders<Client>.IndexKeys.Ascending(field: x => x.Cpf);
        CreateIndexOptions indexOptions = new CreateIndexOptions { Unique = true };
        CreateIndexModel<Client> indexModel = new CreateIndexModel<Client>(keys: indexKeysDefinition, options: indexOptions);
        _clients.Indexes.CreateOne(model: indexModel);

        _bankAccountService = bankAccountService;
        _adressService = adressService;
    }

    public async Task<List<Client>> GetAsync()
    {
        return await _clients.Find(filter: client => true).ToListAsync();
    }

    public async Task<Client> GetAsync(int cpf)
    {
        FilterDefinition<Client>? filter = Builders<Client>.Filter.Eq(field: client => client.Cpf, value: cpf);
        return await _clients.Find(filter: filter).FirstOrDefaultAsync();
    }

    public async Task<Client> CreateAsync(Client client, Adress adress)
    {
        await _clients.InsertOneAsync(document: client);

        // create  a bank account for the client
        BankAccount bankAccount = new BankAccount(cpf: client.Cpf);
        await _bankAccountService.CreateAsync(bankAccount: bankAccount);
        // create adres
        adress.Cpf = client.Cpf;
        await _adressService.CreateAsync(adress: adress);

        return client;
    }
}
