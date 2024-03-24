using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class InvestmentResponseDto
{
   public int Id { get; set; } 
   public string Name { get; set; } = null!;
   public decimal MaintenanceFee { get; set; }
   public decimal MinContribution { get; set; }
   public DateTime MinRedemptionTerm { get; set; }
   public DateTime MaxRedemptionTerm{ get; set; }
   public decimal MinRedemptionValue { get; set; }
   public double RateYield { get; set; }
}
