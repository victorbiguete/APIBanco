using System.ComponentModel.DataAnnotations;
using APIBanco.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace APIBanco.Domain.Models;

public class BankAccount
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Cpf { get; set; } = null!;
    [Required]
    public decimal Balance { get; set; } = 0;
    [Required]
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public virtual List<Transactions> Transactions { get; set; } = new List<Transactions>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? ClientId { get; set; }
    public virtual Client Client { get; set; } = null!;

    public void StatusCheck()
    {
        if (this.Status == AccountStatus.Blocked)
            throw new Exception(message: "Account Blocked.");

        if (this.Status == AccountStatus.Inactive)
            throw new Exception(message: "Account Inactive.");

        if (this.Status == AccountStatus.Closed)
            throw new Exception(message: "Account Closed.");
    }

    public bool Deposit(decimal value)
    {
        this.StatusCheck();

        this.Balance += value;

        this.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public bool Withdraw(decimal value)
    {
        this.StatusCheck();

        if (this.Balance - value < 0)
            throw new ArgumentOutOfRangeException(paramName: "Balance Insufficient.");

        this.Balance -= value;

        this.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public void Transfer(BankAccount destiny, decimal value)
    {
        this.StatusCheck();
        destiny.StatusCheck();

        bool isWithdraw = this.Withdraw(value: value);

        if (isWithdraw)
            destiny.Deposit(value: value);
    }
}
