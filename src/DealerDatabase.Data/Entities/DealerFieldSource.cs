namespace DealerDatabase.Data.Entities;

public class DealerFieldSource
{
    public int Id { get; set; }

    public int DealerId { get; set; }
    public Dealer Dealer { get; set; } = null!;

    public string FieldName { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public string SourceRecordId { get; set; } = string.Empty;
}