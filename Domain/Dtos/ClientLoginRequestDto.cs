using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Dtos;

public class ClientLoginRequestDto
{
    [Required]
    public ulong Cpf { get; set; }
    [Required]
    public string Password { get; set; } = null!;
}
