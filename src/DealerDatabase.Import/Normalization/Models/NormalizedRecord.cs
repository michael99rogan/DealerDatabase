namespace DealerDatabase.Import.Normalization.Models;

public class NormalizedRecord
{
    public required SourceType Source { get; init; }

    public required string SourceRecordId { get; init; }

    public List<string> Names { get; init; } = [];

    public string? LegalName { get; init; }
    public List<string> TradingNames { get; init; } = [];

    public string? CompanyNumber { get; init; }
    public string? VatNumber { get; init; }
    public string? FcaFrn { get; init; }

    public NormalizedAddress? Address { get; init; }

    public List<string> Phones { get; init; } = [];
    public List<string> Emails { get; init; } = [];

    public string? Domain { get; init; }

    public DateOnly? IncorporationDate { get; init; }
    public string? CompanyStatus { get; init; }

    public string? FcaStatus { get; init; }

    public string? IcoRegistrationNumber { get; init; }
    public DateOnly? IcoExpiryDate { get; init; }

    public string? SafStatus { get; init; }
    public DateOnly? SafExpiryDate { get; init; }

    public string? VatValidationStatus { get; init; }

    public string? SourceUrl { get; init; }

    public object? RawRecord { get; init; }
}