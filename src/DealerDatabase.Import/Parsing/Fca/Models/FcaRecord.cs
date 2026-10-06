using System.Text.Json.Serialization;
using DealerDatabase.Import.Parsing.Converters;

namespace DealerDatabase.Import.Parsing.Fca.Models;

public class FcaRecord
{
    [JsonPropertyName("FRN")]
    [JsonConverter(typeof(StringOrNumberConverter))]
    public required string Frn { get; set; }

    [JsonPropertyName("Organisation Name")]
    public string? OrganisationName { get; set; }

    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    [JsonPropertyName("Status Effective Date")]
    public string? StatusEffectiveDate { get; set; }

    [JsonPropertyName("Business Type")]
    public string? BusinessType { get; set; }

    [JsonPropertyName("Companies House Number")]
    public string? CompaniesHouseNumber { get; set; }

    [JsonPropertyName("Trading Names")]
    public List<string> TradingNames { get; set; } = [];

    [JsonPropertyName("Address")]
    public FcaAddress? Address { get; set; }

    [JsonPropertyName("Permissions")]
    public List<string> Permissions { get; set; } = [];

    [JsonPropertyName("Principal Firm")]
    public FcaPrincipalFirm? PrincipalFirm { get; set; }
}