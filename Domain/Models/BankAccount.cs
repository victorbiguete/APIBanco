using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System.Text.Json.Serialization;

using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Models;

[BsonIgnoreExtraElements]
public class BankAccount
{
    [BsonId]
    [BsonRepresentation(representation: BsonType.ObjectId)]
    [JsonPropertyName(name: "id")]
    public string Id { get; set; } = null!;

    [BsonElement(elementName: "cpf")]
    [JsonPropertyName(name: "cpf")]
    public int Cpf { get; set; }

    [BsonElement(elementName: "balance")]
    [JsonPropertyName(name: "balance")]
    public decimal Balance { get; set; }

    [BsonElement(elementName: "status")]
    [JsonPropertyName(name: "status")]
    public AccountStatus Status { get; set; } = AccountStatus.Active;

    [BsonElement(elementName: "created_at")]
    [JsonPropertyName(name: "created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonElement(elementName: "updated_at")]
    [JsonPropertyName(name: "updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public BankAccount(int cpf)
    {
        this.Cpf = cpf;
        this.Balance = 0;
        this.Status = AccountStatus.Active;
    }

    public void StatusCheck()
    {
        if (this.Status == AccountStatus.Blocked)
            throw new Exception(message: "Account Blocked: " + this.Cpf);

        if (this.Status == AccountStatus.Inactive)
            throw new Exception(message: "Account Inactive: " + this.Cpf);

        if (this.Status == AccountStatus.Closed)
            throw new Exception(message: "Account Closed: " + this.Cpf);
    }

    public bool Deposit(decimal value)
    {
        this.StatusCheck();

        this.Balance += value;

        return true;
    }

    public bool Withdraw(decimal value)
    {
        this.StatusCheck();

        if (this.Balance - value < 0)
            throw new ArgumentOutOfRangeException(paramName: "Balance Insufficient: " + this.Cpf);

        this.Balance -= value;

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
