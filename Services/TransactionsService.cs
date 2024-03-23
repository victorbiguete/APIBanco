using APIBanco.Domain.Enums;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using APIBanco.Domain.Models.DbContext;

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

    /// <summary>
    /// Retrieves a transaction by its Id.
    /// </summary>
    /// <param name="Id">The Id of the transaction.</param>
    /// <returns>The transaction with the specified Id.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the transaction is not found.</exception>
    public async Task<Transactions> GetByIdAsync(int Id)
    {
        Transactions? response = await _dbContext.Transactions.AsQueryable().Where(predicate: x => x.Id == Id).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Transaction not found: " + Id);

        return response;
    }

    /// <summary>
    /// Retrieves a list of transactions between a given date range.
    /// </summary>
    /// <param name="GreaterThen">The start date of the range.</param>
    /// <param name="LessThen">The end date of the range. If null, the current date is used.</param>
    /// <returns>A collection of <see cref="Transactions"/> objects..</returns>
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

    /// <summary>
    /// Retrieves a list of transactions associated with a given CPF.
    /// </summary>
    /// <param name="Cpf">The CPF number of the client.</param>
    /// <returns>A list of transactions associated with the given CPF.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no transactions are found for the given CPF.</exception>
    public async Task<IEnumerable<Transactions>> GetByCpfAsync(string Cpf)
    {
        var response = await _dbContext.Transactions.AsQueryable().Where(x => x.Cpf == Cpf).ToListAsync();

        if (response.Count == 0)
            throw new KeyNotFoundException("Transactions not found: " + Cpf);

        return response;
    }

    /// <summary>
    /// Retrieves a list of transactions associated with a given CPF and within a given date range.
    /// </summary>
    /// <param name="Cpf">The CPF number of the client.</param>
    /// <param name="GreaterThen">The start date of the range.</param>
    /// <param name="LessThen">The end date of the range. If null, the current date is used.</param>
    /// <returns>A list of transactions within the given date range and associated with the given CPF.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no transactions are found for the given CPF and date range.</exception>
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

    /// <summary>
    /// Creates a new transaction and updates the corresponding bank account.
    /// </summary>
    /// <param name="Transaction">The transaction to be created.</param>
    /// <param name="Source">The CPF number of the client.</param>
    /// <returns>The created transaction.</returns>
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

    /// <summary>
    /// Creates a new transaction and updates the corresponding bank accounts.
    /// This method is used to perform a transfer between two bank accounts.
    /// </summary>
    /// <param name="Transaction">The transaction to be created.</param>
    /// <param name="Source">The CPF number of the client sending the money.</param>
    /// <param name="Target">The CPF number of the client receiving the money.</param>
    /// <returns>The created transaction.</returns>
    public async Task<Transactions> CreateAsync(Transactions Transaction, string Source, string Target)
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
