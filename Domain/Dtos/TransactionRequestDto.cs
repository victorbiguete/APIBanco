using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class TransactionRequestDto
{
    [Required]
    [RegularExpression(pattern: @"^[0-9]+$")]
    public decimal Value { get; set; }

    public string? Description { get; set; }
}
