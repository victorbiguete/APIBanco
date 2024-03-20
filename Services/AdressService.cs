using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

using APIBanco.Domain.Models;
using ZstdSharp;

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

    public async Task<IEnumerable<Adress>> GetAsync()
    {
        return await _adress.Find(filter: adress => true).ToListAsync();
    }

    public async Task<Adress> GetAsync(string Id)
    {
        FilterDefinition<Adress>? filter = Builders<Adress>.Filter.Eq(field: "_id", value: ObjectId.Parse(s: Id));
        Adress? adress = await _adress.Find(filter: filter).FirstOrDefaultAsync();

        if (adress == null)
            throw new KeyNotFoundException("Adress not found: " + Id);

        return adress;
    }

    public async Task<IEnumerable<Adress>> GetAsync(int cpf)
    {
        FilterDefinition<Adress>? filter = Builders<Adress>.Filter.Eq(field: "Cpf", value: cpf);
        List<Adress>? response = await _adress.Find(filter: filter).ToListAsync();

        if (response.Count == 0)
            throw new KeyNotFoundException("Adress not found: " + cpf);

        return response;
    }

    public async Task<Adress> CreateAsync(Adress adress)
    {
        await _adress.InsertOneAsync(document: adress);
        return adress;
    }

    public async Task<Adress> UpdateAsync(Adress adress)
    {
        FilterDefinition<Adress>? filter = Builders<Adress>.Filter.Eq(field: "_id", value: ObjectId.Parse(s: adress.Id));
        Adress? oldAdress = await _adress.Find(filter: filter).FirstOrDefaultAsync();

        if (oldAdress == null)
            throw new KeyNotFoundException("Adress not found: " + adress.Id);

        adress.Id = oldAdress.Id;
        adress.Cpf = oldAdress.Cpf;
        adress.UpdatedAt = DateTime.Now;

        await _adress.ReplaceOneAsync(filter: filter, replacement: adress);

        return adress;
    }

    public async Task DeleteAsync(string Id)
    {
        FilterDefinition<Adress>? filter = Builders<Adress>.Filter.Eq(field: "_id", value: ObjectId.Parse(s: Id));
        await _adress.DeleteOneAsync(filter: filter);
    }

    public async Task<long> Length()
    {
        long lenght = await _adress.CountDocumentsAsync(filter: new BsonDocument());
        return lenght;
    }
}
