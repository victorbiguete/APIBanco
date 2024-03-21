namespace APIBanco.Domain.Dtos;

public class AdressResponseDto
{
    public int Id { get; set; }
    // public ulong Cpf { get; set; }
    public int ZipCode { get; set; }
    public string Street { get; set; } = null!;
    public int HouseNumber { get; set; }
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    public string UF { get; set; } = null!;
}
