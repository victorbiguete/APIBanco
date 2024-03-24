using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Models.ApiTaskResponses;

public class ApiTaskClientsResponse : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public IEnumerable<ClientResponseDto>? Content { get; set; }
}
