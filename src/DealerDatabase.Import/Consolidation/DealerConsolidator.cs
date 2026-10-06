using DealerDatabase.Data.Entities;
using DealerDatabase.Data.Enums;
using DealerDatabase.Import.Consolidation.Models;
using DealerDatabase.Import.Matching.Models;
using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Consolidation;

public class DealerConsolidator
{
    public ConsolidationResult Consolidate(MatchedGroup group)
    {
        var companySource = SelectSourceRecord(
            group,
            record => record.CompanyNumber,
            SourceType.CompaniesHouse,
            SourceType.Fca,
            SourceType.Ico);

        var vatSource = SelectSourceRecord(
            group,
            record => record.VatNumber,
            SourceType.Vat);

        var fcaSource = SelectSourceRecord(
            group,
            record => record.FcaFrn,
            SourceType.Fca);

        var incorporationSource = SelectSourceRecord(
            group,
            record => record.IncorporationDate,
            SourceType.CompaniesHouse);

        var companyStatusSource = SelectSourceRecord(
            group,
            record => record.CompanyStatus,
            SourceType.CompaniesHouse);

        var fcaStatusSource = SelectSourceRecord(
            group,
            record => record.FcaStatus,
            SourceType.Fca);

        var icoRegistrationSource = SelectSourceRecord(
            group,
            record => record.IcoRegistrationNumber,
            SourceType.Ico);

        var icoExpirySource = SelectSourceRecord(
            group,
            record => record.IcoExpiryDate,
            SourceType.Ico);

        var safStatusSource = SelectSourceRecord(
            group,
            record => record.SafStatus,
            SourceType.Saf);

        var safExpirySource = SelectSourceRecord(
            group,
            record => record.SafExpiryDate,
            SourceType.Saf);

        var vatStatusSource = SelectSourceRecord(
            group,
            record => record.VatValidationStatus,
            SourceType.Vat);

        var legalNameSource = SelectSourceRecord(
            group,
            record => record.LegalName,
            SourceType.CompaniesHouse,
            SourceType.Fca,
            SourceType.Ico,
            SourceType.Saf);

        var legalName = legalNameSource?.LegalName;
        var tradingNames = CollectTradingNames(group, legalName);
        var displayName = SelectDisplayName(group, legalName);

        var dealer = new Dealer
        {
            LegalName = legalName,
            DisplayName = displayName,

            CompanyNumber = companySource?.CompanyNumber,
            IncorporationDate = incorporationSource?.IncorporationDate,
            CompanyStatus = companyStatusSource?.CompanyStatus,

            VatNumber = vatSource?.VatNumber,
            VatValidationStatus = vatStatusSource?.VatValidationStatus,

            FcaFrn = fcaSource?.FcaFrn,
            FcaStatus = fcaStatusSource?.FcaStatus,

            IcoRegistrationNumber =
                icoRegistrationSource?.IcoRegistrationNumber,

            IcoExpiryDate =
                icoExpirySource?.IcoExpiryDate,

            SafStatus =
                safStatusSource?.SafStatus,

            SafExpiryDate =
                safExpirySource?.SafExpiryDate,

            LineageTag = BuildLineageTag(group),

            TradingNames = tradingNames
                .Select(name => new DealerTradingName
                {
                    Name = name
                })
                .ToList(),

            Addresses = CollectAddresses(group),

            Contacts = CollectContacts(group)
        };

        AddFieldSource(
            dealer,
            nameof(Dealer.LegalName),
            legalNameSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.CompanyNumber),
            companySource);

        AddFieldSource(
            dealer,
            nameof(Dealer.IncorporationDate),
            incorporationSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.CompanyStatus),
            companyStatusSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.VatNumber),
            vatSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.VatValidationStatus),
            vatStatusSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.FcaFrn),
            fcaSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.FcaStatus),
            fcaStatusSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.IcoRegistrationNumber),
            icoRegistrationSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.IcoExpiryDate),
            icoExpirySource);

        AddFieldSource(
            dealer,
            nameof(Dealer.SafStatus),
            safStatusSource);

        AddFieldSource(
            dealer,
            nameof(Dealer.SafExpiryDate),
            safExpirySource);

        return new ConsolidationResult
        {
            Dealer = dealer,
            SourceGroup = group
        };
    }

    private static NormalizedRecord? SelectSourceRecord<T>(
    MatchedGroup group,
    Func<NormalizedRecord, T?> selector,
    params SourceType[] preferredSources)
    {
        foreach (var source in preferredSources)
        {
            var record = group.Records
                .Where(record => record.Source == source)
                .FirstOrDefault(record =>
                    selector(record) is not null);

            if (record is not null)
            {
                return record;
            }
        }

        return group.Records
            .FirstOrDefault(record =>
                selector(record) is not null);
    }


    private static string? SelectFirstValue(
    MatchedGroup group,
    Func<NormalizedRecord, string?> selector,
    params SourceType[] preferredSources)
    {
        foreach (var source in preferredSources)
        {
            var value = group.Records
                .Where(record => record.Source == source)
                .Select(selector)
                .FirstOrDefault(value => value is not null);

            if (value is not null)
            {
                return value;
            }
        }

        return group.Records
            .Select(selector)
            .FirstOrDefault(value => value is not null);
    }

    private static string BuildLineageTag(MatchedGroup group)
    {
        return string.Join(
            "|",
            group.Records
                .Select(record =>
                    $"{record.Source}:{record.SourceRecordId}")
                .Distinct()
                .OrderBy(value => value));
    }

    private static List<string> CollectTradingNames(
    MatchedGroup group,
    string? legalName)
    {
        return group.Records
            .SelectMany(record => record.TradingNames)
            .Where(name => name != legalName)
            .Distinct()
            .OrderBy(name => name)
            .ToList();
    }

    private static string? SelectFirstTradingName(
    MatchedGroup group,
    params SourceType[] preferredSources)
    {
        foreach (var source in preferredSources)
        {
            var value = group.Records
                .Where(record => record.Source == source)
                .SelectMany(record => record.TradingNames)
                .FirstOrDefault();

            if (value is not null)
            {
                return value;
            }
        }

        return group.Records
            .SelectMany(record => record.TradingNames)
            .FirstOrDefault();
    }

    private static string? SelectDisplayName(
    MatchedGroup group,
    string? legalName)
    {
        var tradingName = SelectFirstTradingName(
            group,
            SourceType.Fca,
            SourceType.Saf,
            SourceType.MarketCheck,
            SourceType.Crawler,
            SourceType.Ico);

        if (tradingName is not null)
        {
            return tradingName;
        }

        if (legalName is not null)
        {
            return legalName;
        }

        return group.Records
            .SelectMany(record => record.Names)
            .FirstOrDefault();
    }

    private static List<DealerAddress> CollectAddresses(
    MatchedGroup group)
    {
        var addresses = new List<DealerAddress>();

        addresses.AddRange(
            CollectAddresses(
                group,
                AddressType.Registered,
                SourceType.CompaniesHouse));

        AddTradingAddresses(
            addresses,
            group,
            SourceType.MarketCheck);

        AddTradingAddresses(
            addresses,
            group,
            SourceType.Crawler);

        AddTradingAddresses(
            addresses,
            group,
            SourceType.Saf);

        return addresses;
    }

    private static void AddTradingAddresses(
    List<DealerAddress> addresses,
    MatchedGroup group,
    SourceType source)
    {
        var candidates = CollectAddresses(
            group,
            AddressType.Trading,
            source);

        foreach (var candidate in candidates)
        {
            var alreadyRepresented = addresses
                .Where(address =>
                    address.Type == AddressType.Trading)
                .Any(existing =>
                    RepresentsSameLocation(
                        existing,
                        candidate));

            if (!alreadyRepresented)
            {
                addresses.Add(candidate);
            }
        }
    }

    private static bool RepresentsSameLocation(
    DealerAddress existing,
    DealerAddress candidate)
    {
        if (existing.Postcode is not null &&
            candidate.Postcode is not null)
        {
            return existing.Postcode ==
                candidate.Postcode;
        }

        var existingText =
            FlattenAddress(existing);

        var candidateText =
            FlattenAddress(candidate);

        return existingText.Contains(candidateText) ||
            candidateText.Contains(existingText);
    }

    private static string FlattenAddress(
    DealerAddress address)
    {
        return string.Join(
            " ",
            new[]
            {
                address.Line1,
                address.Line2,
                address.Line3,
                address.Town,
                address.County,
                address.Postcode
            }
            .Where(value =>
                !string.IsNullOrWhiteSpace(value)))
            .Replace(" ", "")
            .Replace(",", "")
            .ToUpperInvariant();
    }

    private static IEnumerable<DealerAddress> CollectAddresses(
    MatchedGroup group,
    AddressType type,
    params SourceType[] sources)
    {
        return group.Records
            .Where(record =>
                sources.Contains(record.Source) &&
                record.Address is not null &&
                HasAddressData(record.Address))
            .Select(record => new DealerAddress
            {
                Type = type,

                Line1 = record.Address!.Line1,
                Line2 = record.Address.Line2,
                Line3 = record.Address.Line3,

                Town = record.Address.Town,
                County = record.Address.County,

                Postcode = record.Address.Postcode
            })
            .GroupBy(AddressKey)
            .Select(group => group.First());
    }

    private static bool HasAddressData(
    NormalizedAddress address)
    {
        return
            address.Line1 is not null ||
            address.Line2 is not null ||
            address.Line3 is not null ||
            address.Town is not null ||
            address.County is not null ||
            address.Postcode is not null;
    }

    private static string AddressKey(
    DealerAddress address)
    {
        return string.Join(
            "|",
            address.Line1,
            address.Line2,
            address.Line3,
            address.Town,
            address.County,
            address.Postcode);
    }

    private static List<DealerContact> CollectContacts(
    MatchedGroup group)
    {
        var contacts = new List<DealerContact>();

        contacts.AddRange(
            CollectContacts(
                group,
                ContactType.Phone,
                record => record.Phones));

        contacts.AddRange(
            CollectContacts(
                group,
                ContactType.Email,
                record => record.Emails));

        contacts.AddRange(
            CollectContacts(
                group,
                ContactType.Website,
                record => record.Domain is null
                    ? []
                    : [record.Domain]));

        return contacts;
    }

    private static IEnumerable<DealerContact> CollectContacts(
    MatchedGroup group,
    ContactType type,
    Func<NormalizedRecord, IEnumerable<string>> selector)
    {
        return group.Records
            .SelectMany(selector)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct()
            .OrderBy(value => value)
            .Select(value => new DealerContact
            {
                Type = type,
                Value = value
            });
    }

    private static void AddFieldSource(
    Dealer dealer,
    string fieldName,
    NormalizedRecord? source)
    {
        if (source is null)
        {
            return;
        }

        dealer.FieldSources.Add(
            new DealerFieldSource
            {
                FieldName = fieldName,
                Source = source.Source.ToString(),
                SourceRecordId = source.SourceRecordId
            });
    }
}