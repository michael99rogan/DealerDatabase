using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.Fca.Models;

public class FcaResultInfo
{
    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    [JsonPropertyName("exported")]
    public string? Exported { get; set; }
}