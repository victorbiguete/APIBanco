namespace APIBanco.Domain.Dtos
{
    public class CreditCardResponseDto
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal TotalLimit { get; set; }
        public decimal UsedLimit { get; set; }
    }
}
