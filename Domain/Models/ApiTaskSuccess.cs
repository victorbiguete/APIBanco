namespace APIBanco.Domain.Models;

public class ApiTaskSuccess : IApiTaskResult
{
    public bool Success { get; set; } = true;
    public Object? Content { get; set; }
}
