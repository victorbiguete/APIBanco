using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Models.ApiTaskResponses;

public class ApiTaskAdressesResponse : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public IEnumerable<AdressResponseDto>? Content { get; set; }
}
