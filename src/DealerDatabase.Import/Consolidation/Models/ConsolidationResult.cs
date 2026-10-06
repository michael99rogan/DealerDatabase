using DealerDatabase.Data.Entities;
using DealerDatabase.Import.Matching.Models;

namespace DealerDatabase.Import.Consolidation.Models;

public class ConsolidationResult
{
    public required Dealer Dealer { get; init; }
    public required MatchedGroup SourceGroup { get; init; }

    public List<string> Notes { get; init; } = [];
}