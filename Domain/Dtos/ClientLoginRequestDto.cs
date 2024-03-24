using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class ClientLoginRequestDto
{
    [Required]
    public string Cpf { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
