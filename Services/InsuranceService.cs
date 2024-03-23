using APIBanco.Domain.Contexts;
using APIBanco.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBanco.Services
{
    public class InsuranceService
    {
        private readonly AppDbContext _dbContext;

        public InsuranceService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Insurance>> GetAllInsurancesAsync()
        {
            return await _dbContext.Insurances.ToListAsync();
        }

        public async Task<Insurance> GetInsuranceByIdAsync(int id)
        {
            return await _dbContext.Insurances.FindAsync(id);
        }

        public async Task<Insurance> CreateInsuranceAsync(Insurance insurance)
        {
            _dbContext.Insurances.Add(insurance);
            await _dbContext.SaveChangesAsync();
            return insurance;
        }

        public async Task UpdateInsuranceAsync(Insurance insurance)
        {
            _dbContext.Entry(insurance).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteInsuranceAsync(int id)
        {
            var insurance = await _dbContext.Insurances.FindAsync(id);
            if (insurance == null)
            {
                throw new KeyNotFoundException($"Insurance with ID {id} not found.");
            }

            _dbContext.Insurances.Remove(insurance);
            await _dbContext.SaveChangesAsync();
        }
    }
}
