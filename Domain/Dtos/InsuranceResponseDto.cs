namespace APIBanco.Domain.Dtos
{
    public class InsuranceResponseDto
    {
        public int Id { get; set; }
        public string InsuranceType { get; set; }
        public string Coverage { get; set; }
        public string Company { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Premium { get; set; }
    }
}
