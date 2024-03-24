using System.ComponentModel.DataAnnotations;
using Castle.Components.DictionaryAdapter;

namespace APIBanco.Domain.Dtos;

public class AdressRequestDto
{
    [Required]
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
    [RegularExpression(pattern: @"^[a-zA-Z]{2}$")]
    public string State { get; set; } = null!;
}
