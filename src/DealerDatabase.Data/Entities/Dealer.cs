namespace DealerDatabase.Data.Entities;

/// <summary>
/// A consolidated dealership record.
/// This is a minimal starting point - extend or replace it as you see fit.
/// </summary>
public class Dealer
{
    public int Id { get; set; }
    public string? LegalName { get; set; }
    public string? DisplayName { get; set; }

    public string? CompanyNumber { get; set; }
    public DateOnly? IncorporationDate { get; set; }
    public string? CompanyStatus { get; set; }

    public string? VatNumber { get; set; }
    public string? VatValidationStatus { get; set; }

    public string? FcaFrn { get; set; }
    public string? FcaStatus { get; set; }

    public string? IcoRegistrationNumber { get; set; }
    public DateOnly? IcoExpiryDate { get; set; }

    public string? SafStatus { get; set; }
    public DateOnly? SafExpiryDate { get; set; }

    public string LineageTag { get; set; } = string.Empty;

    public List<DealerTradingName> TradingNames { get; set; } = [];
    public List<DealerAddress> Addresses { get; set; } = [];
    public List<DealerContact> Contacts { get; set; } = [];

    public List<DealerFieldSource> FieldSources { get; set; } = [];
}
