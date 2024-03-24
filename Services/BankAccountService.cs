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

    public async Task<IEnumerable<BankAccount>> GetAsync()
    {
        return await _dbContext.BankAccounts.ToListAsync();
    }

    public async Task<BankAccount> GetByIdAsync(int id)
    {
        BankAccount? response = await _dbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == id);

        if (response == null)
            throw new KeyNotFoundException("BankAccount not found: " + id);

        return response;
    }

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
