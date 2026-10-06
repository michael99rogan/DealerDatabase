using System.Text.Json.Serialization;
using DealerDatabase.Import.Parsing.Converters;

namespace DealerDatabase.Import.Parsing.Fca.Models;

public class FcaPrincipalFirm
{
    [JsonPropertyName("FRN")]
    [JsonConverter(typeof(StringOrNumberConverter))]
    public string? Frn { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }
}