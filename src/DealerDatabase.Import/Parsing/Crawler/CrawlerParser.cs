using System.Globalization;
using CsvHelper;
using DealerDatabase.Import.Parsing.Crawler.Models;

namespace DealerDatabase.Import.Parsing.Crawler;

public class CrawlerParser
{
    public List<CrawlRecord> Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "Crawler data file was not found.",
                filePath);
        }

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(
            reader,
            CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<CrawlRecordMap>();

        return csv.GetRecords<CrawlRecord>().ToList();
    }
}