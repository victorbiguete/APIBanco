using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class AdressRequestDto
{
    [Required]
    // [RegularExpression(pattern: @"^\d{5}-\d{3}$")]
    // TODO remove comment
    public int ZipCode { get; set; }

    [Required]
    public string Street { get; set; } = null!;

    [Required]
    public int HouseNumber { get; set; }

    [Required]
    public string Neighborhood { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    // [RegularExpression(pattern: @"^[a-zA-Z]{2}$")]
    // TODO remove comment
    public string UF { get; set; } = null!;
}