namespace APIBanco.Domain.Dtos;

public class AdressResponseDto
{
    public int Id { get; set; }
    public int Cep { get; set; }
    public string Street { get; set; } = null!;
    public int Number { get; set; }
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
}
