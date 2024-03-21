using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class ClientRequestNoCpfDto
{
    [Required]
    [RegularExpression(@"^[a-zA-Z]+$")]
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
}
