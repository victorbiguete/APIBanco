using APIBanco.Domain.Models;
using APIBanco.Domain.Enums;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace APIBanco.Services;

public class TransactionsService
{
    private readonly AppDbContext _dbContext;
    private BankAccountService _bankAccountService;

    public TransactionsService(AppDbContext dbContext, BankAccountService bankAccountService)
    {
        _dbContext = dbContext;
        _bankAccountService = bankAccountService;
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

    public async Task<IEnumerable<Transactions>> GetByCpfAsync(ulong Cpf)
    {
        var response = await _dbContext.Transactions.AsQueryable().Where(x => x.Cpf == Cpf).ToListAsync();

        if (response.Count == 0)
            throw new KeyNotFoundException("Transactions not found: " + Cpf);

        return response;
    }

    public async Task<IEnumerable<Transactions>> GetAsync(ulong Cpf, DateTime GreaterThen, DateTime? LessThen)
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

    public async Task<Transactions> CreateAsync(Transactions Transaction, ulong Source)
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

    public async Task<Transactions> CreateAsync(Transactions Transaction, ulong Source, ulong Target)
    {
        BankAccount? bankAccountSource = await _bankAccountService.GetByCpfAsync(Cpf: Source);
        BankAccount? bankAccountTarget = await _bankAccountService.GetByCpfAsync(Cpf: Target);

        bankAccountSource.Transfer(destiny: bankAccountTarget, value: Transaction.Value);

        await _bankAccountService.UpdateAsync(bankAccount: bankAccountSource);
        await _bankAccountService.UpdateAsync(bankAccount: bankAccountTarget);

        Transaction.BankAccountId = bankAccountSource.Id;
        await _dbContext.Transactions.AddAsync(Transaction);
        await _dbContext.SaveChangesAsync();

        Transactions transactionTarget = new Transactions(
            cpf: Target,
            description: Transaction.Description ?? " ",
            value: Transaction.Value,
            type: TransactionType.TransferIncome
        );

        transactionTarget.BankAccountId = bankAccountTarget.Id;

        await _dbContext.Transactions.AddAsync(transactionTarget);
        await _dbContext.SaveChangesAsync();

        return Transaction;
    }
}
