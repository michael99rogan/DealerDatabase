namespace DealerDatabase.Import.Normalization.Models;

public class NormalizedAddress
{
    public string? Line1 { get; init; }
    public string? Line2 { get; init; }
    public string? Line3 { get; init; }

    public string? Town { get; init; }
    public string? County { get; init; }

    public string? Postcode { get; init; }
}