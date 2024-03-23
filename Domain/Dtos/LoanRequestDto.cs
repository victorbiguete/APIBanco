using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos
{
    public class LoanRequestDto
    {
        [Required(ErrorMessage = "Loan amount is required.")]
        public decimal LoanAmount { get; set; }

        [Required(ErrorMessage = "Interest rate is required.")]
        public decimal InterestRate { get; set; }

        [Required(ErrorMessage = "Loan term is required.")]
        public int LoanTermMonths { get; set; }

        [Required(ErrorMessage = "Contract date is required.")]
        public DateTime ContractDate { get; set; }
    }
}
