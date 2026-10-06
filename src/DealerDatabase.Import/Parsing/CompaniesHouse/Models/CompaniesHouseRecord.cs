using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.CompaniesHouse.Models;

public class CompaniesHouseRecord
{
    [JsonPropertyName("company_number")]
    public required string CompanyNumber { get; set; }

    [JsonPropertyName("company_name")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("company_status")]
    public string? CompanyStatus { get; set; }

    [JsonPropertyName("type")]
    public string? CompanyType { get; set; }

    [JsonPropertyName("date_of_creation")]
    public string? DateOfCreation { get; set; }

    [JsonPropertyName("date_of_cessation")]
    public string? DateOfCessation { get; set; }

    [JsonPropertyName("registered_office_address")]
    public CompaniesHouseAddress? RegisteredOfficeAddress { get; set; }

    [JsonPropertyName("sic_codes")]
    public List<string> SicCodes { get; set; } = [];

    [JsonPropertyName("officers")]
    public List<CompaniesHouseOfficer> Officers { get; set; } = [];

    [JsonPropertyName("previous_company_names")]
    public List<CompaniesHousePreviousNames> PreviousNames { get; set; } = [];
}