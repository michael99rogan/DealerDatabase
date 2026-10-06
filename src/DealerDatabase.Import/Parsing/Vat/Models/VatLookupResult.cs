using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.Vat.Models;

public class VatLookupResult
{
    // Added for error handling and debugging
    // In event data is missing, corrupted, or not found like in source
    // There is a trace of which file the error occured in
    public string? SourceFileName { get; set; }

    [JsonPropertyName("target")]
    public VatTarget? Target { get; set; }

    [JsonPropertyName("processingDate")]
    public string? ProcessingDate { get; set; }

    [JsonPropertyName("code")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("message")]
    public string? ErrorMessage { get; set; }
}