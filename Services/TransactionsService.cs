using APIBanco.Domain.Enums;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using APIBanco.Domain.Models.DbContext;

namespace APIBanco.Services;

public class TransactionsService
{
    private readonly AppDbContext _dbContext;
    private BankAccountService _bankAccountService;
    private ClientService _clientService;

    public TransactionsService(AppDbContext dbContext, BankAccountService bankAccountService, ClientService clientService)
    {
        _dbContext = dbContext;
        _bankAccountService = bankAccountService;
        _clientService = clientService;
    }

    public async Task<IEnumerable<Transactions>> GetAsync()
    {
        return await _dbContext.Transactions.ToListAsync();
    }

    public async Task<Transactions> GetByIdAsync(int Id)
    {
        Transactions? response = await _dbContext.Transactions.AsQueryable().Where(predicate: x => x.Id == Id).FirstOrDefaultAsync();

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

        List<Transactions>? response = await _dbContext.Transactions
            .AsQueryable()
            .Where(x => x.Date >= GreaterThen && x.Date <= LessThen)
            .ToListAsync();

        return response;
    }

    public async Task<IEnumerable<Transactions>> GetByCpfAsync(string Cpf)
    {
        var response = await _dbContext.Transactions.AsQueryable().Where(x => x.Cpf == Cpf).ToListAsync();

        if (response.Count == 0)
            throw new KeyNotFoundException("Transactions not found: " + Cpf);

        return response;
    }

    public async Task<IEnumerable<Transactions>> GetAsync(string Cpf, DateTime GreaterThen, DateTime? LessThen)
    {
        if (LessThen == null)
        {
            LessThen = DateTime.Now;
        }

        List<Transactions>? response = await _dbContext.Transactions
            .AsQueryable()
            .Where(x => x.Cpf == Cpf && x.Date >= GreaterThen && x.Date <= LessThen)
            .ToListAsync();

        if (response.Count == 0)
            throw new KeyNotFoundException("Transactions not found: " + Cpf);

        return response;
    }

    public async Task<Transactions> CreateAsync(Transactions Transaction, string Source)
    {
        BankAccount? bankAccount = await _bankAccountService.GetByCpfAsync(Cpf: Source);

        if (Transaction.Type == TransactionType.Deposit)
            bankAccount.Deposit(value: Transaction.Value);
        if (Transaction.Type == TransactionType.Withdraw)
            bankAccount.Withdraw(value: Transaction.Value);

        await _bankAccountService.UpdateAsync(bankAccount: bankAccount);

        Transaction.BankAccountId = bankAccount.Id;

        await _dbContext.Transactions.AddAsync(Transaction);
        await _dbContext.SaveChangesAsync();

        return Transaction;
    }

    public async Task<Transactions> CreateAsync(Transactions Transaction, string Source, string Target)
    {
        Client? clientSource = await _clientService.GetByCpfAsync(Cpf: Source);
        Client? clientTarget = await _clientService.GetByCpfAsync(Cpf: Target);
        BankAccount? bankAccountSource = clientSource.BankAccount;
        BankAccount? bankAccountTarget = clientTarget.BankAccount;

        bankAccountSource.Transfer(destiny: bankAccountTarget, value: Transaction.Value);

        await _bankAccountService.UpdateAsync(bankAccount: bankAccountSource);
        await _bankAccountService.UpdateAsync(bankAccount: bankAccountTarget);

        Transaction.BankAccountId = bankAccountSource.Id;
        Transaction.Name = clientTarget.Name;
        await _dbContext.Transactions.AddAsync(Transaction);
        await _dbContext.SaveChangesAsync();

        Transactions transactionTarget = new Transactions(
            cpf: Target,
            description: Transaction.Description ?? " ",
            value: Transaction.Value,
            type: TransactionType.TransferIncome,
            name: clientSource.Name
        );

        transactionTarget.BankAccountId = bankAccountTarget.Id;

        await _dbContext.Transactions.AddAsync(transactionTarget);
        await _dbContext.SaveChangesAsync();

        return Transaction;
    }



    public async Task<IEnumerable<Transactions>?> GetByType(TransactionType type)
    {
        List<Transactions>? transactions = await _dbContext.Transactions
            .AsQueryable()
            .Where(x => x.Type == type)
            .ToListAsync();

        if (transactions.Count == 0)
        {
            return null;
        }

        return transactions;
    }
}
