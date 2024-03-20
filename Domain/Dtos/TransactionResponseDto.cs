using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class TransactionResponseDto
{
    public string Id { get; set; } = null!;
    public int Cpf { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}
