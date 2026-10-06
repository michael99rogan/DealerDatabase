using System.Security.Cryptography.X509Certificates;
using DealerDatabase.Import.Normalization.Common;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Parsing.Ico.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace DealerDatabase.Import.Normalization.Sources;

public class IcoNormalizer
{
    public NormalizedRecord Normalize(IcoRecord record)
    {
        return new NormalizedRecord
        {
            Source = SourceType.Ico,

            SourceRecordId = record.RegistrationNumber,

            Names = NameNormalizer
                .NormalizeMany(
                    record.OrganisationName,
                    record.TradingNames)
                .ToList(),

            LegalName = 
                NameNormalizer.Normalize(record.OrganisationName),

            TradingNames = 
                ParseTradingNames(record.TradingNames)
                    .Where(name => name != NameNormalizer.Normalize(record.OrganisationName))
                    .ToList(),

            CompanyNumber = CompanyNumberNormalizer.Normalize(
                record.CompanyRegistrationNumber
            ),

            Address = new NormalizedAddress
            {
                Line1 = AddressNormalizer.NormalizePart(
                    record.AddressLine1),

                Line2 = AddressNormalizer.NormalizePart(
                    record.AddressLine2),

                Line3 = AddressNormalizer.NormalizePart(
                    record.AddressLine3),

                Postcode = PostcodeNormalizer.Normalize(
                    record.Postcode)
            },

            IcoRegistrationNumber = record.RegistrationNumber,

            IcoExpiryDate = DateNormalizer.NormalizeDate(record.EndDateOfRegistration),

            RawRecord = record
        };
    }

    private static IEnumerable<string> ParseTradingNames(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return value
            .Split(
                [';', '|'],
                StringSplitOptions.RemoveEmptyEntries)
            .Select(NameNormalizer.Normalize)
            .Where(name => name is not null)
            .Distinct()
            .Cast<string>();
    }
}