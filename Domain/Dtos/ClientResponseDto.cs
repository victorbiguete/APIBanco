namespace APIBanco.Domain.Dtos;

public class ClientResponseDto
{
    public int Id { get; set; }
    public ulong Cpf { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime BornDate { get; set; }
    public AdressResponseDto Adress { get; set; } = null!;
    public BankAccountResponseDto BankAccount { get; set; } = null!;
}
