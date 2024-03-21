namespace APIBanco.Domain.Models;

public class ApiTaskErrors : IApiTaskResult
{
    public bool Success { get; set; } = false;
    public IEnumerable<string>? Erros { get; set; }
}
