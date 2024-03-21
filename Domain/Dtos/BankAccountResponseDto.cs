using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Dtos;

public class BankAccountResponseDto
{
    public int Id { get; set; }
    // public ulong Cpf { get; set; }
    public decimal Balance { get; set; }
    public List<TransactionResponseDto> Transactions { get; set; } = null;
    public AccountStatus Status { get; set; }
}
