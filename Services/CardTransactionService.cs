using APIBanco.Domain.Contexts;
using APIBanco.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace APIBanco.Services
{
    public class CardTransactionService
    {
        private readonly AppDbContext _dbContext;

        public CardTransactionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> MakeTransactionAsync(int creditCardId, decimal amount)
        {
            var creditCard = await _dbContext.CreditCards.FindAsync(creditCardId);
            if (creditCard == null)
                return false;

            if (creditCard.TotalLimit - creditCard.UsedLimit < amount)
                return false; // Limite excedido

            var transaction = new CardTransaction
            {
                CreditCardId = creditCardId,
                Amount = amount,
                TransactionDate = DateTime.UtcNow
            };

            creditCard.UsedLimit += amount;

            _dbContext.CardTransactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<CardTransaction>> GetAllTransactionsAsync()
        {
            return await _dbContext.CardTransactions.ToListAsync();
        }

        public async Task<CardTransaction> GetTransactionByIdAsync(int id)
        {
            return await _dbContext.CardTransactions.FindAsync(id);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _dbContext.CardTransactions.FindAsync(id);
            if (transaction != null)
            {
                _dbContext.CardTransactions.Remove(transaction);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Transaction with ID {id} not found.");
            }
        }
    }
}
