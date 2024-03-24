using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Loan amount is required.")]
        public decimal LoanAmount { get; set; }

        [Required(ErrorMessage = "Interest rate is required.")]
        public decimal InterestRate { get; set; }

        [Required(ErrorMessage = "Loan term is required.")]
        public int LoanTermMonths { get; set; }

        public DateTime ContractDate { get; set; }

        public Loan() { }

        public Loan(decimal loanAmount, decimal interestRate, int loanTermMonths, DateTime contractDate)
        {
            LoanAmount = loanAmount;
            InterestRate = interestRate;
            LoanTermMonths = loanTermMonths;
            ContractDate = contractDate;
        }
    }

}

