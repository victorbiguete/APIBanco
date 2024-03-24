namespace APIBanco.Domain.Dtos;

public class AdressResponseDto
{
    public int Id { get; set; }
    public int cep { get; set; }
    public string Street { get; set; } = null!;
    public int number { get; set; }
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
}
