namespace DealerDatabase.Import.Matching.Models;

public class GroupValidationResult
{
    public required MatchedGroup Group { get; init; }

    public bool HasConflict => Conflicts.Count > 0;
    public bool HasWarnings => Warnings.Count > 0;

    public List<string> Conflicts { get; init; } = [];

    public List<GroupValidationWarning> Warnings { get; init; } = [];
}