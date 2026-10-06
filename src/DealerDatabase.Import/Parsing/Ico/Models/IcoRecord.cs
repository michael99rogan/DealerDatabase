namespace DealerDatabase.Import.Parsing.Ico.Models;

public class IcoRecord
{
    public required string RegistrationNumber { get; set; }
    public string? OrganisationName { get; set; }
    public string? CompanyRegistrationNumber { get; set; }

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? Postcode { get; set; }

    public string? PublicAuthority { get; set; }

    public string? StartDateOfRegistration { get; set; }
    public string? EndDateOfRegistration { get; set; }

    public string? PaymentTier { get; set; }
    public string? TradingNames { get; set; }
}