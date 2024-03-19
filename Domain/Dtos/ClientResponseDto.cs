namespace APIBanco.Domain.Dtos;

public class ClientResponseDto
{
    public int Cpf { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int PhoneNumber { get; set; }
    public DateTime BornDate { get; set; }
    public AdressResponseDto Adress { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
