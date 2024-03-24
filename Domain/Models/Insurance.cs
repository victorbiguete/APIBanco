using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Models
{
    public class Insurance
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
