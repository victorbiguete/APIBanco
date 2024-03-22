using System.ComponentModel.DataAnnotations;

namespace APIBanco.Domain.Models;

public class Adress
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Cpf { get; set; } = null!;
    public int ZipCode { get; set; }
    public string Street { get; set; } = null!;
    public int HouseNumber { get; set; }
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    public string UF { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? ClientId { get; set; }
    public virtual Client Client { get; set; } = null!;
}

