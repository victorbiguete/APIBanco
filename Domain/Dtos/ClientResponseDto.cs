namespace APIBanco.Domain.Dtos;

public class ClientResponseDto
{
    public int Id { get; set; }
    public string Cpf { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime BornDate { get; set; }
    public AdressResponseDto Adress { get; set; } = null!;
    public BankAccountResponseDto BankAccount { get; set; } = null!;
}
