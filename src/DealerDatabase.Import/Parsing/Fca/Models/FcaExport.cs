using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.Fca.Models;

public class FcaExport
{
    [JsonPropertyName("Message")]
    public string? Message { get; set; }

    [JsonPropertyName("ResultInfo")]
    public FcaResultInfo? ResultInfo { get; set; }

    [JsonPropertyName("Data")]
    public List<FcaRecord> Data { get; set; } = [];
}