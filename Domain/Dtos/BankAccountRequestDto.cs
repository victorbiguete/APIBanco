using System.ComponentModel.DataAnnotations;
using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class BankAccountRequestDto
{
    [Required]
    public AccountStatus Status { get; set; }
}
