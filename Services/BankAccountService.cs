using APIBanco.Domain.Enums;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using APIBanco.Domain.Models.DbContext;

namespace APIBanco.Services;

public class BankAccountService
{
    private readonly AppDbContext _dbContext;

    public BankAccountService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves all bank accounts from the database asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="BankAccount"/> objects.</returns>
    public async Task<IEnumerable<BankAccount>> GetAsync()
    {
        return await _dbContext.BankAccounts.ToListAsync();
    }

    /// <summary>
    /// Retrieves a bank account by its ID from the database asynchronously.
    /// </summary>
    /// <param name="id">The ID of the bank account.</param>
    /// <returns>The bank account with the specified ID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bank account is not found.</exception>
    public async Task<BankAccount> GetByIdAsync(int id)
    {
        BankAccount? response = await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == id);

        if (response == null)
            throw new KeyNotFoundException("BankAccount not found: " + id);

        return response;
    }

    /// <summary>
    /// Retrieves a bank account by its CPF from the database asynchronously.
    /// </summary>
    /// <param name="Cpf">The CPF of the bank account.</param>
    /// <returns>The bank account with the specified CPF.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bank account is not found.</exception>
    public async Task<BankAccount> GetByCpfAsync(string Cpf)
    {
        BankAccount? response = await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Cpf == Cpf);

        if (response == null)
            throw new KeyNotFoundException("BankAccount not found: " + Cpf);

        return response;
    }

    // public async Task<BankAccount> CreateAsync(BankAccount bankAccount)
    // {
    //     await _dbContext.BankAccounts.AddAsync(bankAccount);
    //     return bankAccount;
    // }

    /// <summary>
    /// Updates a bank account in the database asynchronously.
    /// </summary>
    /// <param name="bankAccount">The bank account to update.</param>
    /// <returns>The updated bank account.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bank account is not found.</exception>
    public async Task<BankAccount> UpdateAsync(BankAccount bankAccount)
    {
        BankAccount? oldBankAccount = await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == bankAccount.Id);

        if (oldBankAccount == null)
            throw new KeyNotFoundException("BankAccount not found: " + bankAccount.Id);

        bankAccount.Id = oldBankAccount.Id;
        bankAccount.UpdatedAt = DateTime.UtcNow;

        _dbContext.BankAccounts.Update(bankAccount);
        await _dbContext.SaveChangesAsync();

        return bankAccount;
    }

    /// <summary>
    /// Updates the status of a bank account in the database asynchronously.
    /// </summary>
    /// <param name="id">The ID of the bank account.</param>
    /// <param name="status">The new status of the bank account.</param>
    /// <returns>The updated bank account.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bank account is not found.</exception>
    public async Task<BankAccount> UpdateStatusAsync(int id, AccountStatus status)
    {
        BankAccount? oldBankAccount = await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == id);

        if (oldBankAccount == null)
            throw new KeyNotFoundException("BankAccount not found: " + id);

        oldBankAccount.Status = status;
        oldBankAccount.UpdatedAt = DateTime.UtcNow;

        _dbContext.Update(oldBankAccount);
        await _dbContext.SaveChangesAsync();

        return oldBankAccount;
    }
}
