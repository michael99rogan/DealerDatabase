using System.Globalization;
using CsvHelper;
using DealerDatabase.Import.Parsing.MarketCheck.Models;

namespace DealerDatabase.Import.Parsing.MarketCheck;

public class MarketCheckParser
{
    public List<MarketCheckRecord> Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "MarketCheck data file was not found.",
                filePath);
        }

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<MarketCheckRecordMap>();

        return csv.GetRecords<MarketCheckRecord>()
                  .ToList();
    }
}