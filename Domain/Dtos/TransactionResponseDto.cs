using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class TransactionResponseDto
{
    public int Id { get; set; }
    public decimal Value { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}
