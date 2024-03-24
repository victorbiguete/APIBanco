namespace APIBanco.Domain.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public ulong CardNumber { get; set; } // Número do cartão (hipotético)
        public string HolderName { get; set; } // Nome do titular do cartão
        public DateTime ExpiryDate { get; set; } // Data de validade do cartão
        public string CVV { get; set; } // Código de segurança do cartão
        public decimal TotalLimit { get; set; } // Limite total do cartão
        public decimal UsedLimit { get; set; } // Limite utilizado do cartão
        public virtual List<CardTransaction> CardTransactions { get; set; }
    }
}
