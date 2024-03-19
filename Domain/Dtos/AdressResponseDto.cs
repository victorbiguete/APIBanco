namespace APIBanco.Domain.Dtos;

public class AdressResponseDto
{
    public int ZipCode { get; set; }
    public string Street { get; set; } = null!;
    public int HouseNumber { get; set; }
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    public string UF { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
