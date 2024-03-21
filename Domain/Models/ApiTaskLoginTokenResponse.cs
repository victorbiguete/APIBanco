namespace APIBanco.Domain.Models;

public class ApiTaskLoginTokenResponse : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public string AuthenticationType { get; set; } = "Jwt Bearer";
    public string TokenType { get; set; } = "Authorization: Bearer";
    public string Token { get; set; } = string.Empty;
}
