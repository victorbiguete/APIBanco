using APIBanco.Domain.Dtos;

namespace APIBanco.Domain.Models.ApiTaskResponses;

public class ApiTaskRegisterResponse : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public string AuthenticationType { get; set; } = "Jwt Bearer";
    public string TokenType { get; set; } = "Authorization: Bearer";
    public string Token { get; set; } = string.Empty;
    public ClientResponseDto Content { get; set; } = null!;
}
