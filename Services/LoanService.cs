using APIBanco.Domain.Contexts;
using APIBanco.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBanco.Services
{
    public class LoanService
    {
        private readonly AppDbContext _dbContext;

        public LoanService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _dbContext.Loans.ToListAsync();
        }

        public async Task<Loan> GetLoanByIdAsync(int id)
        {
            return await _dbContext.Loans.FindAsync(id);
        }

        public async Task<Loan> CreateLoanAsync(Loan loan)
        {
            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();
            return loan;
        }

        public async Task UpdateLoanAsync(Loan loan)
        {
            _dbContext.Entry(loan).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLoanAsync(int id)
        {
            var loan = await _dbContext.Loans.FindAsync(id);
            if (loan == null)
            {
                throw new KeyNotFoundException($"Loan with ID {id} not found.");
            }

            _dbContext.Loans.Remove(loan);
            await _dbContext.SaveChangesAsync();
        }
    }
}
