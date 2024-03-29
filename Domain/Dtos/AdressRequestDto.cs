using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class AdressRequestDto
{
    [Required]
    public int Cep { get; set; }

    [Required]
    public string Street { get; set; } = null!;

    [Required]
    public int Number { get; set; }

    [Required]
    public string Neighborhood { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    [RegularExpression(pattern: @"^[a-zA-Z]{2}$")]
    public string State { get; set; } = null!;
}
