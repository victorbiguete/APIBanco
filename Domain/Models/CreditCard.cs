namespace APIBanco.Domain.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public ulong CardNumber { get; set; } 
        public string? HolderName { get; set; }
        public DateTime ExpiryDate { get; set; } 
        public string? CVV { get; set; }
        public decimal TotalLimit { get; set; }
        public decimal UsedLimit { get; set; }
        public virtual List<CardTransaction>? CardTransactions { get; set; }
    }
}
