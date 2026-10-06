using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.CompaniesHouse.Models;

public class CompaniesHouseOfficer
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("officer_role")]
    public string? OfficerRole { get; set; }

    [JsonPropertyName("appointed_on")]
    public string? AppointedOn { get; set; }

    [JsonPropertyName("resigned_on")]
    public string? ResignedOn { get; set; }

    [JsonPropertyName("occupation")]
    public string? Occupation { get; set; }

    [JsonPropertyName("nationality")]
    public string? Nationality { get; set; }
}