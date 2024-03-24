using System.ComponentModel.DataAnnotations;
namespace APIBanco.Domain.Dtos;

public class InvestmentRequestDto
{
    [Required]  
    public string Name { get; set; } = null!;
    
    [Required]
    public decimal MaintenanceFee { get; set; }

    [Required]
    public decimal MinContribution{ get; set; }

    [Required]
    [DataType(dataType: DataType.Date)]
    public DateTime MinRedemptionTerm { get; set; }

    [Required]
    [DataType(dataType: DataType.Date)]
    public DateTime MaxRedemptionTerm { get; set; }

    [Required]
    public decimal MinRedemptionValue{ get; set; }

    [Required]
    public double RateYield{ get; set; }
    

}
