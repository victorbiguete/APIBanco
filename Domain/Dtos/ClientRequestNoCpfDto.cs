using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class ClientRequestNoCpfDto
{
    [Required]
    public string Name { get; set; } = null!;

    [DataType(dataType: DataType.EmailAddress)]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(length: 8)]
    public string Password { get; set; } = null!;

    [Required]
    public int PhoneNumber { get; set; }

    [DataType(dataType: DataType.Date)]
    [Required]
    public DateTime BornDate { get; set; }
}
