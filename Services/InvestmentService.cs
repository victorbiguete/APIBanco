using APIBanco.Domain.Enums;
using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using APIBanco.Domain.Models.DbContext;

namespace APIBanco.Services;

public class InvestmentService
{
    private readonly AppDbContext _dbContext;

    public InvestmentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Investment>> GetAsync()
    {
        return await _dbContext.Investments.ToListAsync();
    }

     public async Task<Investment> GetByIdAsync(int id)
    {
        Investment? response = await _dbContext.Investments.AsQueryable().Where(predicate: x => x.Id == id).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Investment not found: " + id);

        return response;
    }

    public async Task<Investment> GetByNameAsync(string Name)
    {
        Investment? response = await _dbContext.Investments.AsQueryable().Where(predicate: x => x.Name == Name).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Name not found: " + Name);

        return response;
    }

     public async Task<Investment> GetByStatusAsync(AccountStatus Status)
    {
        Investment? response = await _dbContext.Investments.AsQueryable().Where(predicate: x => x.Status == Status).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Status not found: " + Status);

        return response;
    }

    public async Task<Investment> CreateAsync(Investment Investment)
        {
            if (Investment == null)
            {
                throw new ArgumentNullException(nameof(Investment), "Investment object cannot be null");
            }          

            await _dbContext.Investments.AddAsync(Investment);
            await _dbContext.SaveChangesAsync();

            return Investment;
    }

    public async Task<Investment> UpdateStatusAsync(int id, AccountStatus status)
    {
        Investment? oldInvestment = await _dbContext.Investments.FirstOrDefaultAsync(x => x.Id == id);

        if (oldInvestment == null)
            throw new KeyNotFoundException("Investment not found: " + id);

        oldInvestment.Status = status;

        _dbContext.Investments.Update(oldInvestment);
        await _dbContext.SaveChangesAsync();

        return oldInvestment;
    }

}
