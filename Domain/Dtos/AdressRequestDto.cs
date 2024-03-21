using System.ComponentModel.DataAnnotations;
using Castle.Components.DictionaryAdapter;

namespace APIBanco.Domain.Dtos;

public class AdressRequestDto
{
    [Required]
    [RegularExpression(pattern: @"^[0-9]+$")]
    public int ZipCode { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
    public string Street { get; set; } = null!;

    [Required]
    [RegularExpression(pattern: @"^[0-9]+$")]
    public int HouseNumber { get; set; }

    [Required]
    public string Neighborhood { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    [RegularExpression(pattern: @"^[a-zA-Z]{2}$")]
    public string UF { get; set; } = null!;
}
