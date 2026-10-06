using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.CompaniesHouse.Models;

public sealed class CompaniesHouseExport
{
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("extracted_at")]
    public DateTimeOffset? ExtractedAt { get; set; }

    [JsonPropertyName("items_count")]
    public int ItemsCount { get; set; }

    [JsonPropertyName("items")]
    public List<CompaniesHouseRecord> Items { get; set; } = [];
}