using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class TransactionRequestDto
{
    [Required]
    [RegularExpression(pattern: @"^[0-9]+$")]
    public decimal Value { get; set; }

    [RegularExpression(pattern: @"^[a-zA-Z0-9]+$", ErrorMessage = "Only letters and numbers are allowed.")]
    public string? Description { get; set; }
}
