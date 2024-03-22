using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace APIBanco.Domain.Models;


public class Client
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Key]
    [Required]
    public string Cpf { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime BornDate { get; set; }
    public virtual Adress Adress { get; set; } = new Adress();
    public virtual BankAccount BankAccount { get; set; } = new BankAccount();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
