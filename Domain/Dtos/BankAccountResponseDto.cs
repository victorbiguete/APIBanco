using APIBanco.Domain.Enums;
using MongoDB.Bson;

namespace APIBanco.Domain.Dtos;

public class BankAccountResponseDto
{
    public string Id { get; set; } = null!;
    public int Cpf { get; set; }
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
