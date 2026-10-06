using System.Text.Json;
using DealerDatabase.Import.Parsing.Fca.Models;

namespace DealerDatabase.Import.Parsing.Fca;

public class FcaParser
{
    public FcaExport Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "FCA data file was not found.",
                filePath);
        }

        using var stream = File.OpenRead(filePath);

        var export = JsonSerializer.Deserialize<FcaExport>(stream);

        if (export is null)
        {
            throw new InvalidDataException(
                $"Could not deserialize FCA data from '{filePath}'.");
        }

        if (export.ResultInfo is not null &&
            export.ResultInfo.TotalCount != export.Data.Count)
        {
            throw new InvalidDataException(
                $"FCA export declares {export.ResultInfo.TotalCount} records " +
                $"but contains {export.Data.Count}.");
        }

        return export;
    }
}