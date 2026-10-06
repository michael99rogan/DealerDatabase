using DealerDatabase.Data.Entities;
using DealerDatabase.Data.Enums;

public class DealerAddress
{
    public int Id { get; set; }

    public int DealerId { get; set; }
    public Dealer Dealer { get; set; } = null!;

    public AddressType Type { get; set; }

    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Line3 { get; set; }

    public string? Town { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }
}
