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

    public async Task<List<Transactions>> GetAsync()
    {
        return await _transactions.Find(filter: _ => true).ToListAsync();
    }

    public async Task<List<Transactions>> GetAsync(int cpf)
    {
        FilterDefinition<Transactions>? filter = Builders<Transactions>.Filter.Eq(field: transaction => transaction.Cpf, value: cpf);
        return await _transactions.Find(filter: filter).ToListAsync();
    }

    public async Task<Transactions> CreateAsync(Transactions transaction, int source)
    {
        BankAccount? bankAccount = await _bankAccountService.GetAsync(Cpf: source);

        if (transaction.Type == TransactionType.Deposit)
            bankAccount.Deposit(value: transaction.Value);
        if (transaction.Type == TransactionType.Withdraw)
            bankAccount.Withdraw(value: transaction.Value);

        await _bankAccountService.UpdateAsync(bankAccount: bankAccount);

        await _transactions.InsertOneAsync(document: transaction);
        return transaction;
    }

    public async Task<Transactions> CreateAsync(Transactions transaction, int source, int target)
    {
        BankAccount? bankAccountSource = await _bankAccountService.GetAsync(Cpf: source);
        BankAccount? bankAccountTarget = await _bankAccountService.GetAsync(Cpf: target);

        bankAccountSource.Transfer(destiny: bankAccountTarget, value: transaction.Value);

        await _bankAccountService.UpdateAsync(bankAccount: bankAccountSource);
        await _bankAccountService.UpdateAsync(bankAccount: bankAccountTarget);

        await _transactions.InsertOneAsync(document: transaction);

        Transactions transactionTarget = new Transactions(
            cpf: target,
            description: transaction.Description,
            value: transaction.Value,
            type: TransactionType.TransferIncome
        );

        await _transactions.InsertOneAsync(document: transactionTarget);

        return transaction;
    }

    // public async Task GetByDate(DateTime GreterThen, DateTime LessThen){

    //     FilterDefinition<Transactions>? filter = Builders<Transactions>.Filter.And(
    //         Builders<Transactions>.Filter.Gte("Date", GreterThen),
    //         Builders<Transactions>.Filter.Lte("Date", LessThen)
    //     );

    //     var res = _transactions.Find(Builders<Transactions>.Filter.Eq("Date", DateTime.Now));
    // }


}
