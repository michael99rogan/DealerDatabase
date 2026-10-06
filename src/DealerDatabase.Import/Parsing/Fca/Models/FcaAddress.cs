using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.Fca.Models;

public class FcaAddress
{
    [JsonPropertyName("Address Line 1")]
    public string? AddressLine1 { get; set; }

    [JsonPropertyName("Address Line 2")]
    public string? AddressLine2 { get; set; }

    [JsonPropertyName("Town")]
    public string? Town { get; set; }

    [JsonPropertyName("Postcode")]
    public string? Postcode { get; set; }
}