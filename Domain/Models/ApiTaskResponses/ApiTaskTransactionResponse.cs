using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Models.ApiTaskResponses;

public class ApiTaskTransactionsResponse : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public IEnumerable<TransactionResponseDto>? Content { get; set; }
}
