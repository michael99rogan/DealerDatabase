using DealerDatabase.Import.Normalization.Common;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Parsing.Saf.Models;

namespace DealerDatabase.Import.Normalization.Sources;

public class SafNormalizer
{
    public NormalizedRecord Normalize(SafMember record)
    {
        return new NormalizedRecord
        {
            Source = SourceType.Saf,

            SourceRecordId = record.Id 
                ?? throw new InvalidDataException(
                    "SAF member has no source ID."
                ),

            Names = NameNormalizer.NormalizeMany(
                record.Name,
                record.LegalName,
                record.TradingAs)
                .ToList(),

            LegalName =
                NameNormalizer.Normalize(record.LegalName),

            TradingNames = NameNormalizer
                .NormalizeMany(
                    record.TradingAs,
                    record.Name)
                .ToList(),

            Address = new NormalizedAddress
            {
                Town = AddressNormalizer.NormalizePart(
                    record.Town),

                Postcode = PostcodeNormalizer.Normalize(
                    record.Postcode)
            },

            Phones = PhoneNormalizer
                .NormalizeMany(record.Telephone)
                .ToList(),

            Domain =
                DomainNormalizer.Normalize(record.Website),

            SafStatus = record.Status,

            SafExpiryDate = DateNormalizer.NormalizeDate(record.Expiry),

            RawRecord = record
        };
    }
}