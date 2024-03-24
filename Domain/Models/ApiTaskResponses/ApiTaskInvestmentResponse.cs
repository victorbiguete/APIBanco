using APIBanco.Domain.Dtos;


namespace APIBanco.Domain.Models.ApiTaskResponses;

public class ApiTaskInvestmentResponse : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public IEnumerable<InvestmentResponseDto>? Content { get; set; }
}
