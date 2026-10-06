using System.Globalization;
using CsvHelper;
using DealerDatabase.Import.Parsing.Ico.Models;

namespace DealerDatabase.Import.Parsing.Ico;

public class IcoParser
{
    public List<IcoRecord> Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "ICO data file was not found.",
                filePath);
        }

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<IcoRecordMap>();

        return csv.GetRecords<IcoRecord>().ToList();
    }
}