namespace APIBanco.Domain.Dtos
{
    public class LoanResponseDto
    {
        public int Id { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTermMonths { get; set; }
        public DateTime ContractDate { get; set; }
    }
}
