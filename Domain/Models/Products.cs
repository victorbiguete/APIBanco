using System;
using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Models
{
    public class Products
    {
      
    }

    public class Loan : Products
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

    public class Insurance : Products
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Insurance type is required.")]
        public string InsuranceType { get; set; }

        [Required(ErrorMessage = "Insurance coverage is required.")]
        public string Coverage { get; set; }

        [Required(ErrorMessage = "Insurance company is required.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Policy number is required.")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Insurance start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Insurance end date is required.")]
        public DateTime EndDate { get; set; }

        public decimal Premium { get; set; }
    }
}
