using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class TransactionResponseDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}
