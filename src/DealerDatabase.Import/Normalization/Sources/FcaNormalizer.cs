using DealerDatabase.Import.Normalization.Common;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Parsing.Fca.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace DealerDatabase.Import.Normalization.Sources;

public class FcaNormalizer
{
    public NormalizedRecord Normalize(FcaRecord record)
    {
        return new NormalizedRecord
        {
            Source = SourceType.Fca,

            SourceRecordId = record.Frn,

            Names = NameNormalizer
                .NormalizeMany(
                    [
                        record.OrganisationName,
                        .. record.TradingNames
                    ])
                .ToList(),

            LegalName = NameNormalizer.Normalize(record.OrganisationName),

            TradingNames = record.TradingNames
                .Select(NameNormalizer.Normalize)
                .Where(name => name is not null)
                .Distinct()
                .Cast<string>()
                .ToList(),

            CompanyNumber =
                CompanyNumberNormalizer.Normalize(
                    record.CompaniesHouseNumber),

            FcaFrn =
                FcaFrnNormalizer.Normalize(record.Frn),

            Address = new NormalizedAddress
            {
                Line1 = AddressNormalizer.NormalizePart(
                    record.Address?.AddressLine1),

                Line2 = AddressNormalizer.NormalizePart(
                    record.Address?.AddressLine2),

                Town = AddressNormalizer.NormalizePart(
                    record.Address?.Town),

                Postcode = PostcodeNormalizer.Normalize(
                    record.Address?.Postcode)
            },

            FcaStatus = record.Status,

            RawRecord = record
        };
    }
}