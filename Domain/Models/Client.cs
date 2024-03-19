using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System.Text.Json.Serialization;

namespace APIBanco.Domain.Models;

[BsonIgnoreExtraElements]
public class Client
{
    [BsonId]
    [BsonRepresentation(representation: BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement(elementName: "name")]
    [JsonPropertyName(name: "name")]
    public string Name { get; set; } = null!;

    [BsonElement(elementName: "cpf")]
    [JsonPropertyName(name: "cpf")]
    public int Cpf { get; set; }

    [BsonElement(elementName: "email")]
    [JsonPropertyName(name: "email")]
    public string Email { get; set; } = null!;

    [BsonElement(elementName: "password")]
    [JsonPropertyName(name: "password")]
    public string Password { get; set; } = null!;

    [BsonElement(elementName: "phone_number")]
    [JsonPropertyName(name: "phone_number")]
    public int PhoneNumber { get; set; }

    [BsonElement(elementName: "born_date")]
    [JsonPropertyName(name: "born_date")]
    public DateTime BornDate { get; set; }

    [BsonElement(elementName: "created_at")]
    [JsonPropertyName(name: "created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonElement(elementName: "updated_at")]
    [JsonPropertyName(name: "updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Client() { }

    public Client(string name, int cpf, string email, string password, int phoneNumber, DateTime bornDate)
    {
        this.Name = name;
        this.Cpf = cpf;
        this.Email = email;
        this.Password = password;
        this.PhoneNumber = phoneNumber;
        this.BornDate = bornDate;
    }

}
