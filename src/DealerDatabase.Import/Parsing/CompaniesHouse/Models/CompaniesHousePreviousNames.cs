using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.CompaniesHouse.Models;

public class CompaniesHousePreviousNames
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("effective_from")]
    public string? EffectiveFrom { get; set; }

    [JsonPropertyName("ceased_on")]
    public string? CeasedOn { get; set; }
}