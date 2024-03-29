using System.ComponentModel.DataAnnotations;
using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Models.DbContext;

public class Transactions
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Cpf { get; set; } = null!;
    [Required]
    public decimal Value { get; set; }
    [Required]
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; } = string.Empty;

    public int? BankAccountId { get; set; }
    public virtual BankAccount BankAccount { get; set; } = null!;

    public Transactions() { }

    public Transactions(string cpf, string description, decimal value, TransactionType type, string? name)
    {
        this.Cpf = cpf;
        this.Description = description;
        this.Value = value;
        this.Type = type;
        this.Date = DateTime.UtcNow;
        this.Name = name;
    }
}
