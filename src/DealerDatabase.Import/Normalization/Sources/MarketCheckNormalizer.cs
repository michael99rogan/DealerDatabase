using DealerDatabase.Import.Normalization.Common;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Parsing.MarketCheck.Models;

namespace DealerDatabase.Import.Normalization.Sources;

public class MarketCheckNormalizer
{
    public NormalizedRecord Normalize(MarketCheckRecord record)
    {
        return new NormalizedRecord
        {
            Source = SourceType.MarketCheck,

            SourceRecordId = record.DealerId,

            Names = NameNormalizer
                .NormalizeMany(record.SellerName)
                .ToList(),

            TradingNames = NameNormalizer
                .NormalizeMany(record.SellerName)
                .ToList(),

            Address = new NormalizedAddress
            {
                Line1 = AddressNormalizer.NormalizePart(
                    record.Street),

                Town = AddressNormalizer.NormalizePart(
                    record.City),

                County = AddressNormalizer.NormalizePart(
                    record.County),

                Postcode = PostcodeNormalizer.Normalize(
                    record.Postcode)
            },

            Phones = PhoneNormalizer
                .NormalizeMany(record.Phone)
                .ToList(),

            Emails = EmailNormalizer
                .NormalizeMany(record.Email)
                .ToList(),

            Domain =
                DomainNormalizer.Normalize(record.Website),

            RawRecord = record
        };
    }
}