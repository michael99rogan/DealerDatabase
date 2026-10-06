using DealerDatabase.Import.Normalization.Common;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Parsing.Crawler.Models;

namespace DealerDatabase.Import.Normalization.Sources;

public class CrawlerNormalizer
{
    public NormalizedRecord Normalize(CrawlRecord record)
    {
        return new NormalizedRecord
        {
            Source = SourceType.Crawler,
            SourceRecordId = record.CrawlId,

            Names = NameNormalizer
                .NormalizeMany(record.BusinessNameDetected)
                .ToList(),
            
            TradingNames = NameNormalizer
                .NormalizeMany(record.BusinessNameDetected)
                .ToList(),

            CompanyNumber =
                CompanyNumberNormalizer.Normalize(
                    record.CompanyNumberDetected
                ),

            VatNumber = 
                VatNumberNormalizer.Normalize(
                    record.VatNumberDetected
                ),
            
            FcaFrn = 
                FcaFrnNormalizer.Normalize(
                    record.FcaFrnDetected
                ),

            Address = new NormalizedAddress
            {
                Line1 = AddressNormalizer.NormalizePart(
                    record.AddressDetected),

                Postcode = PostcodeNormalizer.Normalize(
                    record.PostcodeDetected)
            },

            Phones = ParsePhones(record.PhonesDetected)
                .ToList(),

            Emails = ParseEmails(record.EmailsDetected)
                .ToList(),

            Domain =
                DomainNormalizer.Normalize(record.FinalUrl),

            SourceUrl = record.FinalUrl,

            RawRecord = record
        };
    }

    private static IEnumerable<string> ParsePhones(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return value
            .Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries)
            .Select(PhoneNormalizer.Normalize)
            .Where(x => x is not null)
            .Distinct()
            .Cast<string>();
    }

    private static IEnumerable<string> ParseEmails(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return value
            .Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries)
            .Select(EmailNormalizer.Normalize)
            .Where(x => x is not null)
            .Distinct()
            .Cast<string>();
    }
}