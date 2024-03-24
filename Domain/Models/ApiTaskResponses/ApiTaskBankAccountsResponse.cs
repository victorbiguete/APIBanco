using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Models.ApiTaskResponses;

public class ApiTaskBankAccountsResponse : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public IEnumerable<BankAccountResponseDto>? Content { get; set; }
}
