using DealerDatabase.Import.Normalization.Common;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Parsing.CompaniesHouse.Models;

namespace DealerDatabase.Import.Normalization.Sources;

public class CompaniesHouseNormalizer
{
    public NormalizedRecord Normalize(CompaniesHouseRecord record)
    {
        return new NormalizedRecord
        {
            Source = SourceType.CompaniesHouse,

            SourceRecordId = record.CompanyNumber,

            Names = NameNormalizer
                .NormalizeMany(record.CompanyName)
                .ToList(),

            LegalName = NameNormalizer.Normalize(record.CompanyName),

            CompanyNumber =
                CompanyNumberNormalizer.Normalize(
                    record.CompanyNumber),

            Address = new NormalizedAddress
            {
                Line1 = AddressNormalizer.NormalizePart(
                    record.RegisteredOfficeAddress?.AddressLine1),

                Line2 = AddressNormalizer.NormalizePart(
                    record.RegisteredOfficeAddress?.AddressLine2),

                Town = AddressNormalizer.NormalizePart(
                    record.RegisteredOfficeAddress?.Locality),

                County = AddressNormalizer.NormalizePart(
                    record.RegisteredOfficeAddress?.Region),

                Postcode = PostcodeNormalizer.Normalize(
                    record.RegisteredOfficeAddress?.PostalCode)
            },

            CompanyStatus = record.CompanyStatus,

            IncorporationDate = DateNormalizer.NormalizeDate(record.DateOfCreation),

            RawRecord = record
        };
    }
}