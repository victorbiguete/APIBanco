namespace APIBanco.Domain.Dtos
{
    public class CreditCardRequestDto
    {
        public string? CardNumber { get; set; }
        public string? HolderName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? CVV { get; set; }
        public decimal TotalLimit { get; set; }
    }
}
