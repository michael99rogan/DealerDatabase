using System.Text.Json;
using DealerDatabase.Import.Parsing.CompaniesHouse.Models;

namespace DealerDatabase.Import.Parsing.CompaniesHouse;

public class CompaniesHouseParser
{
    public CompaniesHouseExport Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "Companies House data file was not found.",
                filePath
            );
        }

        using var stream = File.OpenRead(filePath);

        var export = JsonSerializer.Deserialize<CompaniesHouseExport>(stream);

        if (export is null)
        {
            throw new InvalidDataException(
                $"Could not deserialize Companies House data from '{filePath}'."
            );
        }

        if (export.ItemsCount != export.Items.Count)
        {
            throw new InvalidDataException(
                $"Companies House export declares {export.ItemsCount} items" +
                $"but contains {export.Items.Count}."
            );
        }

        return export;
    }
}