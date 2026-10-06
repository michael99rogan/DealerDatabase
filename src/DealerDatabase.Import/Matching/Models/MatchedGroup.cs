using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Matching.Models;

public class MatchedGroup
{
    public List<NormalizedRecord> Records { get; init; } = [];
}