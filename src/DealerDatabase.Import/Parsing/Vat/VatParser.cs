using System.Runtime.CompilerServices;
using System.Text.Json;
using DealerDatabase.Import.Parsing.Vat.Models;

namespace DealerDatabase.Import.Parsing.Vat;

public class VatParser
{
    public List<VatLookupResult> Parse(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException(
                $"VAT lookups directory was not found: '{directoryPath}'.");
        }

        var results = new List<VatLookupResult>();

        // Order files deterministically for easier debugging
        var filePaths = Directory
            .EnumerateFiles(directoryPath, "*.json")
            .OrderBy(path => path);

        foreach (var filePath in filePaths)
        {
            var result = ParseFile(filePath);
            results.Add(result);
        }

        return results;
    }

    private static VatLookupResult ParseFile(string filePath)
    {
        using var stream = File.OpenRead(filePath);

        var result = JsonSerializer.Deserialize<VatLookupResult>(stream)
            ?? throw new InvalidDataException(
                $"Could not deserialize VAT lookup from '{filePath}'."
            );

        // Assign the filename to the result item for data source investigation if needed
        result.SourceFileName = Path.GetFileName(filePath);

        return result;
    }
}