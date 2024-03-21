using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class ClientRequestDto
{
    [Required]
    [RegularExpression(pattern: @"^[0-9]+$")]
    public ulong Cpf { get; set; }

    [Required]
    [RegularExpression(pattern: @"^[a-zA-Z]+$")]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(dataType: DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [DataType(dataType: DataType.Date)]
    public DateTime BornDate { get; set; }

    [Required]
    public AdressRequestDto Adress { get; set; } = null!;
}
