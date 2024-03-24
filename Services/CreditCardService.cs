using APIBanco.Domain.Contexts;
using APIBanco.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace APIBanco.Services
{
    public class CreditCardService
    {
        private readonly AppDbContext _context;

        public CreditCardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CreditCard> CreateCreditCardAsync(CreditCard creditCard)
        {
            _context.CreditCards.Add(creditCard);
            await _context.SaveChangesAsync();
            return creditCard;
        }

        public async Task<CreditCard> GetCreditCardByIdAsync(int id)
        {
            return await _context.CreditCards.FindAsync(id);
        }

        public async Task<List<CreditCard>> GetAllCreditCardsAsync()
        {
            return await _context.CreditCards.ToListAsync();
        }

        public async Task DeleteCreditCardAsync(int id)
        {
            var creditCard = await GetCreditCardByIdAsync(id);

            if (creditCard != null)
            {
                _context.CreditCards.Remove(creditCard);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Credit card with ID {id} not found.");
            }
        }

        public async Task<CreditCard> ProcessTransactionAsync(CardTransaction transaction)
        {
            var creditCard = await _context.CreditCards.FindAsync(transaction.CreditCardId);
            if (creditCard == null)
            {
                throw new InvalidOperationException($"Credit card with ID {transaction.CreditCardId} not found.");
            }

            if (creditCard.TotalLimit - creditCard.UsedLimit < transaction.Amount)
            {
                throw new InvalidOperationException("Insufficient credit limit.");
            }

            creditCard.UsedLimit += transaction.Amount;
            _context.CardTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return creditCard;
        }
    }
}
