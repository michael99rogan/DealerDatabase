using DealerDatabase.Data.Entities;
using DealerDatabase.Data.Enums;

public class DealerContact
{
    public int Id { get; set; }

    public int DealerId { get; set; }
    public Dealer Dealer { get; set; } = null!;

    public ContactType Type { get; set; }

    public string Value { get; set; } = string.Empty;
}