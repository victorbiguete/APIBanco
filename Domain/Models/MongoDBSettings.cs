namespace APIBanco.Domain.Models;

public class MongoDBSettings
{
    public string? ConnectionURI { get; set; }
    public string? DatabaseName { get; set; }
    public string? CollectionClient { get; set; }
    public string? CollectionBankAccount { get; set; }
    public string? CollectionTransactions { get; set; }
    public string? CollectionAdresses { get; set; }
}
