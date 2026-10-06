using DealerDatabase.Import.Matching;
using DealerDatabase.Import.Matching.Models;
using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Tests.Matching;

public class GroupValidatorTests
{
    private readonly GroupValidator _validator = new();

    [Fact]
    public void Validate_MatchingAuthoritativeCompanyNumbers_HasNoConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"),
            CreateRecord(
                SourceType.Fca,
                "FCA-1",
                companyNumber: "12345678"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.Empty(result.Conflicts);
    }

    [Fact]
    public void Validate_DifferentAuthoritativeCompanyNumbers_HasConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"),
            CreateRecord(
                SourceType.Fca,
                "FCA-1",
                companyNumber: "87654321"));

        var result = _validator.Validate(group);

        Assert.True(result.HasConflict);
        Assert.Single(result.Conflicts);

        Assert.Contains(
            "Conflicting authoritative company numbers",
            result.Conflicts[0]);
    }

    [Fact]
    public void Validate_DifferentCrawlerCompanyNumber_DoesNotCauseConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                companyNumber: "87654321"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.Empty(result.Conflicts);
    }

    [Fact]
    public void Validate_DifferentAuthoritativeVatNumbers_HasConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Vat,
                "VAT-1",
                vatNumber: "111111111"),
            CreateRecord(
                SourceType.Vat,
                "VAT-2",
                vatNumber: "222222222"));

        var result = _validator.Validate(group);

        Assert.True(result.HasConflict);
        Assert.Single(result.Conflicts);

        Assert.Contains(
            "Conflicting authoritative VAT numbers",
            result.Conflicts[0]);
    }

    [Fact]
    public void Validate_DifferentCrawlerVatNumber_DoesNotCauseConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Vat,
                "VAT-1",
                vatNumber: "111111111"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                vatNumber: "222222222"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.Empty(result.Conflicts);
    }

    [Fact]
    public void Validate_DifferentAuthoritativeFcaFrns_HasConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "FCA-1",
                fcaFrn: "123456"),
            CreateRecord(
                SourceType.Fca,
                "FCA-2",
                fcaFrn: "654321"));

        var result = _validator.Validate(group);

        Assert.True(result.HasConflict);
        Assert.Single(result.Conflicts);

        Assert.Contains(
            "Conflicting authoritative FCA FRNs",
            result.Conflicts[0]);
    }

    [Fact]
    public void Validate_DifferentCrawlerFcaFrn_DoesNotCauseConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "FCA-1",
                fcaFrn: "123456"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                fcaFrn: "654321"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.Empty(result.Conflicts);
    }

    [Fact]
    public void Validate_OpenshawStyleCrawlerFcaMismatch_HasNoConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "920089",
                companyNumber: "08398392",
                fcaFrn: "920089"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00095",
                companyNumber: "08398392",
                fcaFrn: "628633"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.Empty(result.Conflicts);
    }

    [Fact]
    public void Validate_SingleRecord_HasNoConflict()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.Empty(result.Conflicts);
    }

    [Fact]
    public void Validate_EmptyGroup_HasNoConflict()
    {
        var group = new MatchedGroup();

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.Empty(result.Conflicts);
    }

    [Fact]
    public void Validate_MultipleTypesOfConflict_ReturnsAllReasons()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"),
            CreateRecord(
                SourceType.Fca,
                "FCA-1",
                companyNumber: "87654321",
                fcaFrn: "123456"),
            CreateRecord(
                SourceType.Fca,
                "FCA-2",
                companyNumber: "87654321",
                fcaFrn: "654321"),
            CreateRecord(
                SourceType.Vat,
                "VAT-1",
                vatNumber: "111111111"),
            CreateRecord(
                SourceType.Vat,
                "VAT-2",
                vatNumber: "222222222"));

        var result = _validator.Validate(group);

        Assert.True(result.HasConflict);
        Assert.Equal(3, result.Conflicts.Count);

        Assert.Contains(
            result.Conflicts,
            reason => reason.Contains(
                "Conflicting authoritative company numbers"));

        Assert.Contains(
            result.Conflicts,
            reason => reason.Contains(
                "Conflicting authoritative VAT numbers"));

        Assert.Contains(
            result.Conflicts,
            reason => reason.Contains(
                "Conflicting authoritative FCA FRNs"));
    }

    [Fact]
    public void Validate_CrawlerCompanyNumberDisagreesWithAuthoritative_AddsWarning()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                companyNumber: "87654321"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.True(result.HasWarnings);

        var warning = Assert.Single(result.Warnings);

        Assert.Equal(SourceType.Crawler, warning.Source);
        Assert.Equal("CRW-1", warning.SourceRecordId);
        Assert.Equal("CompanyNumber", warning.Field);
        Assert.Equal("87654321", warning.ObservedValue);
        Assert.Equal("12345678", warning.AuthoritativeValue);
    }

    [Fact]
    public void Validate_CrawlerVatDisagreesWithAuthoritative_AddsWarning()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Vat,
                "GB969336568.json",
                vatNumber: "969336568"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00141",
                vatNumber: "969363568"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.True(result.HasWarnings);

        var warning = Assert.Single(result.Warnings);

        Assert.Equal(SourceType.Crawler, warning.Source);
        Assert.Equal("CRW-00141", warning.SourceRecordId);
        Assert.Equal("VatNumber", warning.Field);
        Assert.Equal("969363568", warning.ObservedValue);
        Assert.Equal("969336568", warning.AuthoritativeValue);
    }

    [Fact]
    public void Validate_CrawlerFcaDisagreesWithAuthoritative_AddsWarning()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "920089",
                companyNumber: "08398392",
                fcaFrn: "920089"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00095",
                companyNumber: "08398392",
                fcaFrn: "628633"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.True(result.HasWarnings);

        var warning = Assert.Single(result.Warnings);

        Assert.Equal(SourceType.Crawler, warning.Source);
        Assert.Equal("CRW-00095", warning.SourceRecordId);
        Assert.Equal("FcaFrn", warning.Field);
        Assert.Equal("628633", warning.ObservedValue);
        Assert.Equal("920089", warning.AuthoritativeValue);
    }

    [Fact]
    public void Validate_CrawlerValuesAgreeWithAuthoritative_HasNoWarnings()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.CompaniesHouse,
                "CH-1",
                companyNumber: "12345678"),
            CreateRecord(
                SourceType.Vat,
                "VAT-1",
                vatNumber: "111111111"),
            CreateRecord(
                SourceType.Fca,
                "FCA-1",
                companyNumber: "12345678",
                fcaFrn: "123456"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-1",
                companyNumber: "12345678",
                vatNumber: "111111111",
                fcaFrn: "123456"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.False(result.HasWarnings);
        Assert.Empty(result.Conflicts);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void Validate_MultipleCrawlerDisagreements_AddsWarningForEachRecord()
    {
        var group = CreateGroup(
            CreateRecord(
                SourceType.Fca,
                "920089",
                fcaFrn: "920089"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00094",
                fcaFrn: "628633"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00095",
                fcaFrn: "628633"),
            CreateRecord(
                SourceType.Crawler,
                "CRW-00096",
                fcaFrn: "628633"));

        var result = _validator.Validate(group);

        Assert.False(result.HasConflict);
        Assert.True(result.HasWarnings);
        Assert.Equal(3, result.Warnings.Count);

        Assert.All(
            result.Warnings,
            warning =>
            {
                Assert.Equal(SourceType.Crawler, warning.Source);
                Assert.Equal("FcaFrn", warning.Field);
                Assert.Equal("628633", warning.ObservedValue);
                Assert.Equal("920089", warning.AuthoritativeValue);
            });
    }

    private static MatchedGroup CreateGroup(
        params NormalizedRecord[] records)
    {
        return new MatchedGroup
        {
            Records = records.ToList()
        };
    }

    private static NormalizedRecord CreateRecord(
        SourceType source,
        string sourceRecordId,
        string? companyNumber = null,
        string? vatNumber = null,
        string? fcaFrn = null)
    {
        return new NormalizedRecord
        {
            Source = source,
            SourceRecordId = sourceRecordId,
            CompanyNumber = companyNumber,
            VatNumber = vatNumber,
            FcaFrn = fcaFrn
        };
    }
}