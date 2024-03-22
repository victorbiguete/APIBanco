namespace APIBanco.Domain.Models;

public class TokenIdNotEqualsClientIdException : Exception
{
    public TokenIdNotEqualsClientIdException() { }
    public TokenIdNotEqualsClientIdException(string message) : base(message) { }
}
