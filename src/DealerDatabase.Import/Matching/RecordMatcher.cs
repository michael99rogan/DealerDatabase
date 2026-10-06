using DealerDatabase.Import.Matching.Models;
using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Matching;

public class RecordMatcher
{
    private readonly MatchScorer _scorer;

    public RecordMatcher(MatchScorer scorer)
    {
        _scorer = scorer;
    }

    public List<MatchedGroup> Match(
        IReadOnlyList<NormalizedRecord> records)
    {
        var graph = new Dictionary<int, List<int>>();

        // Every normalized record becomes a node in the graph.
        for (var i = 0; i < records.Count; i++)
        {
            graph[i] = [];
        }

        // Compare every unique pair and create an edge
        // whenever MatchScorer accepts the relationship.
        for (var i = 0; i < records.Count; i++)
        {
            for (var j = i + 1; j < records.Count; j++)
            {
                var result = _scorer.Compare(
                    records[i],
                    records[j]);

                if (!result.IsMatch)
                {
                    continue;
                }

                graph[i].Add(j);
                graph[j].Add(i);
            }
        }

        // Find the connected components of the graph.
        var visited = new HashSet<int>();
        var groups = new List<MatchedGroup>();

        for (var i = 0; i < records.Count; i++)
        {
            if (visited.Contains(i))
            {
                continue;
            }

            var groupRecords = new List<NormalizedRecord>();
            var stack = new Stack<int>();

            stack.Push(i);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current))
                {
                    continue;
                }

                groupRecords.Add(records[current]);

                foreach (var neighbour in graph[current])
                {
                    if (!visited.Contains(neighbour))
                    {
                        stack.Push(neighbour);
                    }
                }
            }

            groups.Add(new MatchedGroup
            {
                Records = groupRecords
            });
        }

        return groups;
    }
}