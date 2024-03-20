using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

using APIBanco.Domain.Models;
using APIBanco.Domain.Enums;

namespace APIBanco.Services;

public class TransactionsService
{
    private IMongoCollection<Transactions> _transactions;

    private BankAccountService _bankAccountService;

    public TransactionsService(IOptions<MongoDBSettings> settings, BankAccountService bankAccountService)
    {
        MongoClient? cliente = new MongoClient(connectionString: settings.Value.ConnectionURI);
        IMongoDatabase? database = cliente.GetDatabase(name: settings.Value.DatabaseName);
        _transactions = database.GetCollection<Transactions>(name: settings.Value.CollectionTransactions);

        _bankAccountService = bankAccountService;
    }

    public async Task<IEnumerable<Transactions>> GetAsync()
    {
        return await _transactions.Find(filter: _ => true).ToListAsync();
    }

    public async Task<Transactions> GetAsync(string Id)
    {
        FilterDefinition<Transactions>? filter = Builders<Transactions>.Filter.Eq(field: "_id", value: ObjectId.Parse(s: Id));
        Transactions? response = await _transactions.Find(filter: filter).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Transaction not found: " + Id);

        return response;
    }

    public async Task<IEnumerable<Transactions>> GetAsync(DateTime GreaterThen, DateTime? LessThen)
    {
        if (LessThen == null)
        {
            LessThen = DateTime.Now;
        }

        FilterDefinition<Transactions>? filter = Builders<Transactions>.Filter.And(
            Builders<Transactions>.Filter.Gte(field: "Date", value: GreaterThen),
            Builders<Transactions>.Filter.Lte(field: "Date", value: LessThen)
        );

        IEnumerable<Transactions>? response = await _transactions.Find(filter: filter).ToListAsync();

        return response;
    }

    public async Task<IEnumerable<Transactions>> GetAsync(int Cpf)
    {
        FilterDefinition<Transactions>? filter = Builders<Transactions>.Filter.Eq(field: transaction => transaction.Cpf, value: Cpf);
        return await _transactions.Find(filter: filter).ToListAsync();
    }

    public async Task<IEnumerable<Transactions>> GetAsync(int Cpf, DateTime GreaterThen, DateTime? LessThen)
    {
        if (LessThen == null)
        {
            LessThen = DateTime.Now;
        }

        FilterDefinition<Transactions>? filter = Builders<Transactions>.Filter.And(
            Builders<Transactions>.Filter.Gte(field: "Date", value: GreaterThen),
            Builders<Transactions>.Filter.Lte(field: "Date", value: LessThen),
            Builders<Transactions>.Filter.Eq(field: "Cpf", value: Cpf)
        );

        List<Transactions>? response = await _transactions.Find(filter: filter).ToListAsync();

        if (response.Count == 0)
            throw new KeyNotFoundException("Transactions not found: " + Cpf);

        return response;
    }

    public async Task<Transactions> CreateAsync(Transactions Transaction, int Source)
    {
        IEnumerable<BankAccount>? bankAccount = await _bankAccountService.GetAsync(Cpf: Source);

        if (Transaction.Type == TransactionType.Deposit)
            bankAccount.First().Deposit(value: Transaction.Value);
        if (Transaction.Type == TransactionType.Withdraw)
            bankAccount.First().Withdraw(value: Transaction.Value);

        await _bankAccountService.UpdateAsync(bankAccount: bankAccount.First());

        await _transactions.InsertOneAsync(document: Transaction);
        return Transaction;
    }

    public async Task<Transactions> CreateAsync(Transactions Transaction, int Source, int Target)
    {
        IEnumerable<BankAccount>? bankAccountSource = await _bankAccountService.GetAsync(Cpf: Source);
        IEnumerable<BankAccount>? bankAccountTarget = await _bankAccountService.GetAsync(Cpf: Target);


        bankAccountSource.First().Transfer(destiny: bankAccountTarget.First(), value: Transaction.Value);

        await _bankAccountService.UpdateAsync(bankAccount: bankAccountSource.First());
        await _bankAccountService.UpdateAsync(bankAccount: bankAccountTarget.First());

        await _transactions.InsertOneAsync(document: Transaction);

        Transactions transactionTarget = new Transactions(
            cpf: Target,
            description: Transaction.Description,
            value: Transaction.Value,
            type: TransactionType.TransferIncome
        );

        await _transactions.InsertOneAsync(document: transactionTarget);

        return Transaction;
    }

    public async Task<long> Length()
    {
        long lenght = await _transactions.CountDocumentsAsync(filter: new BsonDocument());
        return lenght;
    }

}
