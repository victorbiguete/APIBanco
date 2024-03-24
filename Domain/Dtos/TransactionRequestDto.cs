using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class TransactionRequestDto
{
    [Required]
    public decimal Value { get; set; }

    public string? Description { get; set; }
}
