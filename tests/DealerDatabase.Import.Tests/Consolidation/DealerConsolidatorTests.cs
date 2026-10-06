using DealerDatabase.Data.Entities;
using DealerDatabase.Data.Enums;
using DealerDatabase.Import.Consolidation;
using DealerDatabase.Import.Matching.Models;
using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Tests.Consolidation;

public class DealerConsolidatorTests
{
    private readonly DealerConsolidator _consolidator = new();

    private static NormalizedRecord CreateRecord(
        SourceType source,
        string sourceRecordId,
        string? companyNumber = null,
        string? vatNumber = null,
        string? fcaFrn = null,
        string? legalName = null,
        string[]? tradingNames = null,
        DateOnly? incorporationDate = null,
        string? companyStatus = null,
        string? fcaStatus = null,
        string? icoRegistrationNumber = null,
        DateOnly? icoExpiryDate = null,
        string? vatValidationStatus = null,
        string? safStatus = null,
        DateOnly? safExpiryDate = null,
        NormalizedAddress? address = null,
        string[]? phones = null,
        string[]? emails = null,
        string? domain = null,
        string[]? names = null)
    {
        return new NormalizedRecord
        {
            Source = source,
            SourceRecordId = sourceRecordId,
            CompanyNumber = companyNumber,
            VatNumber = vatNumber,
            FcaFrn = fcaFrn,
            LegalName = legalName,
            TradingNames = tradingNames?.ToList() ?? [],
            IncorporationDate = incorporationDate,
            CompanyStatus = companyStatus,
            FcaStatus = fcaStatus,
            IcoRegistrationNumber = icoRegistrationNumber,
            IcoExpiryDate = icoExpiryDate,
            SafStatus = safStatus,
            SafExpiryDate = safExpiryDate,
            VatValidationStatus = vatValidationStatus,
            Address = address,
            Phones = phones?.ToList() ?? [],
            Emails = emails?.ToList() ?? [],
            Domain = domain,
            Names = names?.ToList() ?? []
        };
    }

    private static MatchedGroup CreateGroup(
        params NormalizedRecord[] records)
    {
        return new MatchedGroup
        {
            Records = records.ToList()
        };
    }

    [Fact]
    public void Consolidate_PrefersCompaniesHouseCompanyNumber()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                companyNumber: "99999999"),
            CreateRecord(
                SourceType.Ico,
                "ICO-1",
                companyNumber: "88888888"),
            CreateRecord(
                SourceType.Fca,
                "FCA-1",
                companyNumber: "77777777"),
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "12345678",
            result.Dealer.CompanyNumber);
    }

    [Fact]
    public void Consolidate_PrefersVatLookupOverCrawlerVat()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Crawler,
                "CRW-00141",
                vatNumber: "969363568"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00142",
                vatNumber: "969336568"),
            CreateRecord(
                SourceType.Vat,
                "GB969336568.json",
                vatNumber: "969336568"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "969336568",
            result.Dealer.VatNumber);
    }

    [Fact]
    public void Consolidate_PrefersFcaRegisterOverCrawlerFca()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Crawler,
                "CRW-00094",
                companyNumber: "08398392",
                fcaFrn: "628633"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00095",
                companyNumber: "08398392",
                fcaFrn: "628633"),
            CreateRecord(
                SourceType.Fca,
                "920089",
                companyNumber: "08398392",
                fcaFrn: "920089"),
            CreateRecord(
                SourceType.CompaniesHouse,
                "08398392",
                companyNumber: "08398392"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "08398392",
            result.Dealer.CompanyNumber);

        Assert.Equal(
            "920089",
            result.Dealer.FcaFrn);
    }

    [Fact]
    public void Consolidate_UsesAvailableFcaAsFallback_WhenFcaSourceMissing()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                fcaFrn: "123456"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "123456",
            result.Dealer.FcaFrn);
    }

    [Fact]
    public void Consolidate_NoIdentifiers_ReturnsNullIdentifiers()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1"));

        var result = _consolidator.Consolidate(group);

        Assert.Null(result.Dealer.CompanyNumber);
        Assert.Null(result.Dealer.VatNumber);
        Assert.Null(result.Dealer.FcaFrn);
    }

    [Fact]
    public void Consolidate_BuildsDeterministicLineageTag()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "920089"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00095"),
            CreateRecord(
                SourceType.CompaniesHouse,
                "08398392"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "CompaniesHouse:08398392|" +
            "Crawler:CRW-00095|" +
            "Fca:920089",
            result.Dealer.LineageTag);
    }

    [Fact]
    public void Consolidate_LineageTagDoesNotDependOnRecordOrder()
    {
        var first = CreateRecord(
            SourceType.CompaniesHouse,
            "08398392");

        var second = CreateRecord(
            SourceType.Fca,
            "920089");

        var resultA = _consolidator.Consolidate(
            CreateGroup(first, second));

        var resultB = _consolidator.Consolidate(
            CreateGroup(second, first));

        Assert.Equal(
            resultA.Dealer.LineageTag,
            resultB.Dealer.LineageTag);
    }

    [Fact]
    public void Consolidate_SeparatesLegalAndTradingNames()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "08398392",
                legalName: "DAWSON HOLDINGS LIMITED"),
            CreateRecord(
                SourceType.Fca,
                "920089",
                legalName: "DAWSON HOLDINGS LTD",
                tradingNames: ["OPENSHAW MOTOR COMPANY"]),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00095",
                tradingNames: ["OPENSHAW MOTOR COMPANY"]));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "DAWSON HOLDINGS LIMITED",
            result.Dealer.LegalName);

        Assert.Equal(
            "OPENSHAW MOTOR COMPANY",
            result.Dealer.DisplayName);

        var tradingName = Assert.Single(
            result.Dealer.TradingNames);

        Assert.Equal(
            "OPENSHAW MOTOR COMPANY",
            tradingName.Name);
    }

    [Fact]
    public void Consolidate_NoTradingName_UsesLegalNameAsDisplayName()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "12345678",
                legalName: "EXAMPLE MOTORS LIMITED"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "EXAMPLE MOTORS LIMITED",
            result.Dealer.LegalName);

        Assert.Equal(
            "EXAMPLE MOTORS LIMITED",
            result.Dealer.DisplayName);

        Assert.Empty(result.Dealer.TradingNames);
    }

    [Fact]
    public void Consolidate_DuplicateTradingNames_AreDeduplicated()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "123456",
                tradingNames: ["EXAMPLE MOTORS"]),
            CreateRecord(
                SourceType.Saf,
                "SAF-1",
                tradingNames: ["EXAMPLE MOTORS"]),
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                tradingNames: ["EXAMPLE MOTORS"]));

        var result = _consolidator.Consolidate(group);

        var tradingName = Assert.Single(
            result.Dealer.TradingNames);

        Assert.Equal(
            "EXAMPLE MOTORS",
            tradingName.Name);
    }

    [Fact]
    public void Consolidate_PrefersCompaniesHouseLegalName()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "123456",
                legalName: "EXAMPLE MOTORS LTD"),
            CreateRecord(
                SourceType.CompaniesHouse,
                "12345678",
                legalName: "EXAMPLE MOTORS LIMITED"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "EXAMPLE MOTORS LIMITED",
            result.Dealer.LegalName);
    }

    [Fact]
    public void Consolidate_UsesCompaniesHouseCompanyMetadata()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "15253958",
                companyNumber: "15253958",
                incorporationDate:
                    new DateOnly(2023, 10, 16),
                companyStatus: "active"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            new DateOnly(2023, 10, 16),
            result.Dealer.IncorporationDate);

        Assert.Equal(
            "active",
            result.Dealer.CompanyStatus);
    }

    [Fact]
    public void Consolidate_UsesFcaStatusFromFcaSource()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "920089",
                fcaFrn: "920089",
                fcaStatus: "Authorised"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "Authorised",
            result.Dealer.FcaStatus);
    }

    [Fact]
    public void Consolidate_UsesIcoRegistrationAndExpiry()
    {
        var expiry = new DateOnly(2027, 4, 12);

        var group = CreateGroup(
            CreateRecord(
                SourceType.Ico,
                "ZA978203",
                icoRegistrationNumber: "ZA978203",
                icoExpiryDate: expiry));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "ZA978203",
            result.Dealer.IcoRegistrationNumber);

        Assert.Equal(
            expiry,
            result.Dealer.IcoExpiryDate);
    }

    [Fact]
    public void Consolidate_PreservesSafAndVatMetadata()
    {
        var safExpiry = new DateOnly(2027, 6, 30);

        var group = CreateGroup(
            CreateRecord(
                SourceType.Saf,
                "SAF-95000",
                safStatus: "Active",
                safExpiryDate: safExpiry),
            CreateRecord(
                SourceType.Vat,
                "GB415548648.json",
                vatNumber: "415548648",
                vatValidationStatus: "VALID"));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "Active",
            result.Dealer.SafStatus);

        Assert.Equal(
            safExpiry,
            result.Dealer.SafExpiryDate);

        Assert.Equal(
            "415548648",
            result.Dealer.VatNumber);

        Assert.Equal(
            "VALID",
            result.Dealer.VatValidationStatus);
    }

    [Fact]
    public void Consolidate_MissingRegulatorySources_LeavesMetadataNull()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                tradingNames: ["EXAMPLE MOTORS"]));

        var result = _consolidator.Consolidate(group);

        Assert.Null(result.Dealer.IncorporationDate);
        Assert.Null(result.Dealer.CompanyStatus);
        Assert.Null(result.Dealer.FcaStatus);
        Assert.Null(result.Dealer.IcoRegistrationNumber);
        Assert.Null(result.Dealer.IcoExpiryDate);
        Assert.Null(result.Dealer.SafStatus);
        Assert.Null(result.Dealer.SafExpiryDate);
        Assert.Null(result.Dealer.VatValidationStatus);
    }

    [Fact]
    public void Consolidate_CompaniesHouseAddress_IsRegistered()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "12345678",
                address: new NormalizedAddress
                {
                    Line1 = "10 HIGH STREET",
                    Town = "SUNDERLAND",
                    Postcode = "SR11AA"
                }));

        var result = _consolidator.Consolidate(group);

        var address =
            Assert.Single(result.Dealer.Addresses);

        Assert.Equal(
            AddressType.Registered,
            address.Type);

        Assert.Equal(
            "10 HIGH STREET",
            address.Line1);

        Assert.Equal(
            "SUNDERLAND",
            address.Town);

        Assert.Equal(
            "SR11AA",
            address.Postcode);
    }

    [Fact]
    public void Consolidate_MarketCheckAddress_IsTrading()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                address: new NormalizedAddress
                {
                    Line1 = "20 MARKET ROAD",
                    Town = "NEWCASTLE",
                    Postcode = "NE11AA"
                }));

        var result = _consolidator.Consolidate(group);

        var address =
            Assert.Single(result.Dealer.Addresses);

        Assert.Equal(
            AddressType.Trading,
            address.Type);
    }

    [Fact]
    public void Consolidate_DuplicateTradingAddresses_AreDeduplicated()
    {
        var address = new NormalizedAddress
        {
            Line1 = "20 MARKET ROAD",
            Town = "NEWCASTLE",
            Postcode = "NE11AA"
        };

        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                address: address),

            CreateRecord(
                SourceType.Saf,
                "SAF-1",
                address: address),

            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                address: address));

        var result =
            _consolidator.Consolidate(group);

        var consolidatedAddress =
            Assert.Single(result.Dealer.Addresses);

        Assert.Equal(
            AddressType.Trading,
            consolidatedAddress.Type);
    }

    [Fact]
    public void Consolidate_SameRegisteredAndTradingAddress_PreservesBothTypes()
    {
        var address = new NormalizedAddress
        {
            Line1 = "20 MARKET ROAD",
            Town = "NEWCASTLE",
            Postcode = "NE11AA"
        };

        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "12345678",
                address: address),

            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                address: address));

        var result =
            _consolidator.Consolidate(group);

        Assert.Equal(
            2,
            result.Dealer.Addresses.Count);

        Assert.Contains(
            result.Dealer.Addresses,
            address =>
                address.Type == AddressType.Registered);

        Assert.Contains(
            result.Dealer.Addresses,
            address =>
                address.Type == AddressType.Trading);
    }

    [Fact]
    public void Consolidate_EmptyAddress_IsIgnored()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                address: new NormalizedAddress()));

        var result =
            _consolidator.Consolidate(group);

        Assert.Empty(result.Dealer.Addresses);
    }

    [Fact]
    public void Consolidate_CollectsContactDetails()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                phones: ["441915551234"],
                emails: ["sales@example.com"],
                domain: "example.com"));

        var result = _consolidator.Consolidate(group);

        Assert.Contains(
            result.Dealer.Contacts,
            contact =>
                contact.Type == ContactType.Phone &&
                contact.Value == "441915551234");

        Assert.Contains(
            result.Dealer.Contacts,
            contact =>
                contact.Type == ContactType.Email &&
                contact.Value == "sales@example.com");

        Assert.Contains(
            result.Dealer.Contacts,
            contact =>
                contact.Type == ContactType.Website &&
                contact.Value == "example.com");
    }

    [Fact]
    public void Consolidate_DuplicateContactsAcrossSources_AreDeduplicated()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                phones: ["441915551234"],
                emails: ["sales@example.com"],
                domain: "example.com"),

            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                phones: ["441915551234"],
                emails: ["sales@example.com"],
                domain: "example.com"),

            CreateRecord(
                SourceType.Saf,
                "SAF-1",
                phones: ["441915551234"],
                domain: "example.com"));

        var result = _consolidator.Consolidate(group);

        Assert.Single(
            result.Dealer.Contacts,
            contact =>
                contact.Type == ContactType.Phone);

        Assert.Single(
            result.Dealer.Contacts,
            contact =>
                contact.Type == ContactType.Email);

        Assert.Single(
            result.Dealer.Contacts,
            contact =>
                contact.Type == ContactType.Website);
    }

    [Fact]
    public void Consolidate_DifferentPhoneNumbers_ArePreserved()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                phones:
                [
                    "441915551111",
                    "441915552222"
                ]),

            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                phones:
                [
                    "441915551111",
                    "441915553333"
                ]));

        var result = _consolidator.Consolidate(group);

        var phones = result.Dealer.Contacts
            .Where(contact =>
                contact.Type == ContactType.Phone)
            .Select(contact => contact.Value)
            .ToList();

        Assert.Equal(3, phones.Count);

        Assert.Contains("441915551111", phones);
        Assert.Contains("441915552222", phones);
        Assert.Contains("441915553333", phones);
    }

    [Fact]
    public void Consolidate_EmptyContacts_AreIgnored()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                phones: ["", "   "],
                emails: ["", "   "]));

        var result = _consolidator.Consolidate(group);

        Assert.Empty(result.Dealer.Contacts);
    }

    [Fact]
    public void Consolidate_TradingAddressesWithSamePostcode_AreDeduplicated()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                address: new NormalizedAddress
                {
                    Line1 = "27 BROOK LANE",
                    Town = "MACCLESFIELD",
                    Postcode = "SK107YR"
                }),

            CreateRecord(
                SourceType.Saf,
                "SAF-1",
                address: new NormalizedAddress
                {
                    Town = "MACCLESFIELD",
                    Postcode = "SK107YR"
                }));

        var result = _consolidator.Consolidate(group);

        var address = Assert.Single(
            result.Dealer.Addresses);

        Assert.Equal(
            "27 BROOK LANE",
            address.Line1);
    }

    [Fact]
    public void Consolidate_CrawlerAddressContainedInStructuredAddress_IsDeduplicated()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                address: new NormalizedAddress
                {
                    Line1 = "27 BROOK LANE",
                    Town = "MACCLESFIELD",
                    Postcode = "SK107YR"
                }),

            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                address: new NormalizedAddress
                {
                    Line1 =
                        "27 BROOK LANE, MACCLESFIELD, SK10 7YR"
                }));

        var result = _consolidator.Consolidate(group);

        Assert.Single(
            result.Dealer.Addresses);
    }

    [Fact]
    public void Consolidate_DifferentTradingLocations_ArePreserved()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.MarketCheck,
                "MC-1",
                address: new NormalizedAddress
                {
                    Line1 = "27 BROOK LANE",
                    Town = "MACCLESFIELD",
                    Postcode = "SK107YR"
                }),

            CreateRecord(
                SourceType.MarketCheck,
                "MC-2",
                address: new NormalizedAddress
                {
                    Line1 = "214 QUEENS ROAD",
                    Town = "WARRINGTON",
                    Postcode = "WA18BF"
                }));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            2,
            result.Dealer.Addresses.Count);
    }

    [Fact]
    public void Consolidate_NameWithUnknownSemantics_IsUsedAsDisplayFallback()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Vat,
                "GB148294150.json",
                names: ["PENDLE VEHICLE SALES LIMITED"]));

        var result = _consolidator.Consolidate(group);

        Assert.Equal(
            "PENDLE VEHICLE SALES LIMITED",
            result.Dealer.DisplayName);

        Assert.Null(
            result.Dealer.LegalName);

        Assert.Empty(
            result.Dealer.TradingNames);
    }

    [Fact]
    public void Consolidate_RecordsIdentifierProvenance()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                companyNumber: "05780177"),

            CreateRecord(
                SourceType.CompaniesHouse,
                "05780177",
                companyNumber: "05780177"));

        var result =
            _consolidator.Consolidate(group);

        var provenance = Assert.Single(
            result.Dealer.FieldSources,
            source =>
                source.FieldName ==
                nameof(Dealer.CompanyNumber));

        Assert.Equal(
            SourceType.CompaniesHouse.ToString(),
            provenance.Source);

        Assert.Equal(
            "05780177",
            provenance.SourceRecordId);
    }

    [Fact]
    public void Consolidate_RecordsMetadataProvenance()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "983390",
                fcaFrn: "983390",
                fcaStatus: "Authorised"));

        var result =
            _consolidator.Consolidate(group);

        Assert.Equal(
            "Authorised",
            result.Dealer.FcaStatus);

        var source = Assert.Single(
            result.Dealer.FieldSources,
            source =>
                source.FieldName ==
                nameof(Dealer.FcaStatus));

        Assert.Equal(
            SourceType.Fca.ToString(),
            source.Source);

        Assert.Equal(
            "983390",
            source.SourceRecordId);
    }
}