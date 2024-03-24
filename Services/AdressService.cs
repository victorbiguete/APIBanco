using APIBanco.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using APIBanco.Domain.Models.DbContext;

namespace APIBanco.Services;

public class AdressService
{
    private AppDbContext _dbContext;

    public AdressService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Adress>> GetAsync()
    {
        List<Adress>? response = await _dbContext.Adresses.ToListAsync();

        return response;
    }

    public async Task<Adress> GetByIdAsync(int id)
    {
        Adress? response = await _dbContext.Adresses.AsQueryable().Where(predicate: x => x.Id == id).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Adress not found: " + id);

        return response;
    }

    public async Task<Adress> GetByCpfAsync(string cpf)
    {
        Adress? response = await _dbContext.Adresses.AsQueryable().Where(predicate: x => x.Cpf == cpf).FirstOrDefaultAsync();

        if (response == null)
            throw new KeyNotFoundException("Adress not found: " + cpf);

        return response;
    }

    public async Task<Adress> CreateAsync(Adress adress)
    {
        await _dbContext.Adresses.AddAsync(adress);
        _dbContext.SaveChanges();

        return adress;
    }

    public async Task<Adress> UpdateAsync(Adress adress)
    {
        Adress? oldAdress = await _dbContext.Adresses.AsQueryable().Where(predicate: x => x.Id == adress.Id).FirstOrDefaultAsync();

        if (oldAdress == null)
            throw new KeyNotFoundException("Adress not found: " + adress.Id);

        adress.Id = oldAdress.Id;
        adress.Cpf = oldAdress.Cpf;
        adress.UpdatedAt = DateTime.UtcNow;

        _dbContext.Adresses.Update(adress);
        await _dbContext.SaveChangesAsync();

        return adress;
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.Adresses.Where(predicate: x => x.Id == id).ExecuteDeleteAsync();
        await _dbContext.SaveChangesAsync();
    }
}
