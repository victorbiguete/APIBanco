using System.ComponentModel.DataAnnotations;
using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Models.DbContext
{
    public class Investment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]  
        public string Name { get; set; } = null!;

        // TaxaManutenção
        [Required]
        public decimal MaintenanceFee { get; set; }

        // AporteMinimo
        [Required]
        public decimal MinContribution{ get; set; }
        
        // prazoMinimoResgate
        [Required]
        public DateTime MinRedemptionTerm { get; set; }

        // prazoMinimoResgate
        [Required]
        public DateTime MaxRedemptionTerm { get; set; }

        // valorResgateMinimo
        [Required]
        public decimal MinRedemptionValue{ get; set; }

        // taxaRendimento
        [Required]
        public double RateYield{ get; set; }
    }
}