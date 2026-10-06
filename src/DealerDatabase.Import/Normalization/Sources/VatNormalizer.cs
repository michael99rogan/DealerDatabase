using DealerDatabase.Import.Normalization.Common;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Parsing.Vat.Models;

namespace DealerDatabase.Import.Normalization.Sources;

public class VatNormalizer
{
    private const string ValidVatStatus = "VALID";

    public NormalizedRecord? Normalize(VatLookupResult record)
    {
        if (record.Target is null)
        {
            return null;
        }

        return new NormalizedRecord
        {
            Source = SourceType.Vat,

            SourceRecordId = record.SourceFileName 
                ?? throw new InvalidDataException(
                    "VAT lookup has no source filename."
                ),

            Names = NameNormalizer.NormalizeMany(
                record.Target?.Name)
                .ToList(),
            
            LegalName = null,

            TradingNames = [],
            
            VatNumber = VatNumberNormalizer.Normalize(
                record.Target?.VatNumber
            ),

            Address = new NormalizedAddress
            {
                Line1 = AddressNormalizer.NormalizePart(
                    record.Target?.Address?.Line1),

                Line2 = AddressNormalizer.NormalizePart(
                    record.Target?.Address?.Line2),

                Line3 = AddressNormalizer.NormalizePart(
                    record.Target?.Address?.Line3),

                Postcode = PostcodeNormalizer.Normalize(
                    record.Target?.Address?.Postcode)
            },

            VatValidationStatus = ValidVatStatus,

            RawRecord = record
        };
    }
}