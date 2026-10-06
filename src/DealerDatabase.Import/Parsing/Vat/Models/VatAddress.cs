using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.Vat.Models;

public class VatAddress
{
    [JsonPropertyName("line1")]
    public string? Line1 { get; set; }

    [JsonPropertyName("line2")]
    public string? Line2 { get; set; }

    [JsonPropertyName("line3")]
    public string? Line3 { get; set; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }
}