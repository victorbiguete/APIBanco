namespace APIBanco.Domain.Models.Exceptions;

public class TokenIdNotEqualsClientIdException : Exception
{
    public TokenIdNotEqualsClientIdException() { }
    public TokenIdNotEqualsClientIdException(string message) : base(message) { }
}
