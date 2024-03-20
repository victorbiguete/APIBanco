using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System.Text.Json.Serialization;

namespace APIBanco.Domain.Models;

public class Adress
{
    [BsonId]
    [BsonRepresentation(representation: BsonType.ObjectId)]
    [JsonPropertyName(name: "id")]
    public string Id { get; set; } = null!;

    [BsonElement(elementName: "cpf")]
    [JsonPropertyName(name: "cpf")]
    public int Cpf { get; set; }

    [BsonElement(elementName: "zip_code")]
    [JsonPropertyName(name: "zip_code")]
    public int ZipCode { get; set; }

    [BsonElement(elementName: "street")]
    [JsonPropertyName(name: "street")]
    public string Street { get; set; } = null!;

    [BsonElement(elementName: "house_number")]
    [JsonPropertyName(name: "house_number")]
    public int HouseNumber { get; set; }

    [BsonElement(elementName: "neighborhood")]
    [JsonPropertyName(name: "neighborhood")]
    public string Neighborhood { get; set; } = null!;

    [BsonElement(elementName: "city")]
    [JsonPropertyName(name: "city")]
    public string City { get; set; } = null!;

    [BsonElement(elementName: "uf")]
    [JsonPropertyName(name: "uf")]
    public string UF { get; set; } = null!;

    [BsonElement(elementName: "created_at")]
    [JsonPropertyName(name: "created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonElement(elementName: "updated_at")]
    [JsonPropertyName(name: "updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

