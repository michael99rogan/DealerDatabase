namespace DealerDatabase.Import.Parsing.Saf.Models;

public class SafExport
{
    public string? Exported { get; set; }

    public List<SafMember> Members { get; set; } = [];
}