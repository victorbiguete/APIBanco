using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System.Text.Json.Serialization;

using APIBanco.Domain.Enums;

namespace APIBanco.Domain.Models;

[BsonIgnoreExtraElements]
public class Transactions
{
    [BsonId]
    [BsonRepresentation(representation: BsonType.ObjectId)]
    [JsonPropertyName(name: "id")]
    public string Id { get; set; } = null!;

    [BsonElement(elementName: "cpf")]
    [JsonPropertyName(name: "cpf")]
    public int Cpf { get; set; }

    [BsonElement(elementName: "description")]
    [JsonPropertyName(name: "description")]
    public string Description { get; set; } = null!;

    [BsonElement(elementName: "value")]
    [JsonPropertyName(name: "value")]
    public decimal Value { get; set; }

    [BsonElement(elementName: "date")]
    [JsonPropertyName(name: "date")]
    public DateTime Date { get; set; }

    [BsonElement(elementName: "type")]
    [JsonPropertyName(name: "type")]
    public TransactionType Type { get; set; }

    public Transactions() { }

    public Transactions(int cpf, string description, decimal value, TransactionType type)
    {
        this.Cpf = cpf;
        this.Description = description;
        this.Value = value;
        this.Date = DateTime.Now;
        this.Type = type;
    }
}
