using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class ClientRequestDto
{
    [Required]
    // [RegularExpression(pattern: @"^\d{11}$")]
    // TODO: remover comentario
    public int Cpf { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [DataType(dataType: DataType.EmailAddress)]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(length: 8)]
    public string Password { get; set; } = null!;

    // [DataType(dataType: DataType.PhoneNumber)]
    // TODO: remover comentario
    [Required]
    public int PhoneNumber { get; set; }

    [DataType(dataType: DataType.Date)]
    [Required]
    public DateTime BornDate { get; set; }

    public AdressRequestDto Adress { get; set; } = null!;
}
