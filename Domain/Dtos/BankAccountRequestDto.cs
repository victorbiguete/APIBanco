using System.ComponentModel.DataAnnotations;
using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class BankAccountRequestDto
{
    [Required]
    [RegularExpression(pattern: @"^[0-9]+$")]
    public AccountStatus Status { get; set; }
}
