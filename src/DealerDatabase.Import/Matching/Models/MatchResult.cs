using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Matching.Models;

public class MatchResult
{
    public required NormalizedRecord Left { get; init; }

    public required NormalizedRecord Right { get; init; }

    public int Score { get; init; }

    public bool IsMatch { get; init; }

    public bool HasConflict { get; init; }

    public List<string> Reasons { get; init; } = [];
}