using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

using APIBanco.Domain.Models;

namespace APIBanco.Services;

public class AdressService
{
    private IMongoCollection<Adress> _adress;

    public AdressService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient? mongoClient = new MongoClient(connectionString: mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase? mongoDatabase = mongoClient.GetDatabase(name: mongoDBSettings.Value.DatabaseName);
        _adress = mongoDatabase.GetCollection<Adress>(name: mongoDBSettings.Value.CollectionAdresses);

        // make  cpf be unique
        IndexKeysDefinition<Adress>? indexKeysDefinition = Builders<Adress>.IndexKeys.Ascending(field: x => x.Cpf);
        CreateIndexOptions indexOptions = new CreateIndexOptions { Unique = true };
        CreateIndexModel<Adress> indexModel = new CreateIndexModel<Adress>(keys: indexKeysDefinition, options: indexOptions);
        _adress.Indexes.CreateOne(model: indexModel);
    }

    public async Task<List<Adress>> GetAsync()
    {
        return await _adress.Find(filter: adress => true).ToListAsync();
    }

    public async Task<Adress> GetAsync(int cpf)
    {
        FilterDefinition<Adress>? filter = Builders<Adress>.Filter.Eq(field: "Cpf", value: cpf);
        return await _adress.Find(filter: filter).FirstOrDefaultAsync();
    }

    public async Task<Adress> CreateAsync(Adress adress)
    {
        await _adress.InsertOneAsync(document: adress);
        return adress;
    }

    public async Task<Adress> UpdateAsync(Adress adress)
    {
        FilterDefinition<Adress>? filter = Builders<Adress>.Filter.Eq(field: "Cpf", value: adress.Cpf);
        Adress? oldAdress = _adress.Find(filter: filter).FirstOrDefault();

        adress.Id = oldAdress.Id;
        adress.UpdatedAt = DateTime.Now;

        await _adress.ReplaceOneAsync(filter: filter, replacement: adress);

        return adress;
    }
}
