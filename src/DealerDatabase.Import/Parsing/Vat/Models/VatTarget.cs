using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.Vat.Models;

public class VatTarget
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("vatNumber")]
    public string? VatNumber { get; set; }

    [JsonPropertyName("address")]
    public VatAddress? Address { get; set; }
}