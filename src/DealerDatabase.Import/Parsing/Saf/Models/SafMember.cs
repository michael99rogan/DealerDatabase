namespace DealerDatabase.Import.Parsing.Saf.Models;

public class SafMember
{
    public string? Id { get; set; }

    public string? Name { get; set; }
    public string? LegalName { get; set; }
    public string? TradingAs { get; set; }

    public string? Town { get; set; }
    public string? Postcode { get; set; }

    public string? Telephone { get; set; }
    public string? Website { get; set; }

    public string? Status { get; set; }
    public string? Expiry { get; set; }
}