using DealerDatabase.Import.Normalization.Models;

public class GroupValidationWarning
{
    public required SourceType Source { get; init; }
    public required string SourceRecordId { get; init; }

    public required string Field { get; init; }

    public required string ObservedValue { get; init; }
    public required string AuthoritativeValue { get; init; }
}