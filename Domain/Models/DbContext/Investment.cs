using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Models.DbContext
{
    public class Investment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]  
        public string Name { get; set; } = null!;

        [Required]
        public decimal MaintenanceFee { get; set; }

        [Required]
        public decimal MinContribution{ get; set; }
        
        [Required]
        public DateTime MinRedemptionTerm { get; set; }

        [Required]
        public DateTime MaxRedemptionTerm { get; set; }

        [Required]
        public decimal MinRedemptionValue{ get; set; }

        [Required]
        public double RateYield{ get; set; }
    }
}