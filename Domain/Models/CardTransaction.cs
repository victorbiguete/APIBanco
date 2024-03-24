namespace APIBanco.Domain.Models
{
    public class CardTransaction
    {
        public int Id { get; set; }
        public int CreditCardId { get; set; }
        public virtual CreditCard CreditCard { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
