using DealerDatabase.Import.Matching;
using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Tests.Matching;

public class MatchScorerTests
{
    [Fact]
    public void Compare_SameCompanyNumber_ReturnsMatch()
    {
        var left = CreateRecord(
            companyNumber: "05860635");

        var right = CreateRecord(
            companyNumber: "05860635");

        var scorer = new MatchScorer();

        var result = scorer.Compare(left, right);

        Assert.True(result.IsMatch);
        Assert.Equal(100, result.Score);
    }

    [Fact]
    public void Compare_SameNameOnly_DoesNotReturnMatch()
    {
        var left = CreateRecord(
            names: ["PREMIER MOTORS"]);

        var right = CreateRecord(
            names: ["PREMIER MOTORS"]);

        var scorer = new MatchScorer();

        var result = scorer.Compare(left, right);

        Assert.False(result.IsMatch);
        Assert.Equal(40, result.Score);
    }

    [Fact]
    public void Compare_DomainAndPostcode_ReturnsMatch()
    {
        var left = CreateRecord(
            domain: "examplecars.co.uk",
            postcode: "NE61AA");

        var right = CreateRecord(
            domain: "examplecars.co.uk",
            postcode: "NE61AA");

        var scorer = new MatchScorer();

        var result = scorer.Compare(left, right);

        Assert.True(result.IsMatch);
        Assert.Equal(100, result.Score);
    }

    [Fact]
    public void Compare_ConflictingCompanyNumbers_DoesNotMatch()
    {
        var left = CreateRecord(
            companyNumber: "01234567",
            domain: "examplecars.co.uk",
            names: ["EXAMPLE CARS"]);

        var right = CreateRecord(
            companyNumber: "87654321",
            domain: "examplecars.co.uk",
            names: ["EXAMPLE CARS"]);

        var scorer = new MatchScorer();

        var result = scorer.Compare(left, right);

        Assert.False(result.IsMatch);
        Assert.True(result.HasConflict);
    }

    [Fact]
    public void Compare_MatchingCompanyNumberWithConflictingCrawlerFrn_ReturnsMatch()
    {
        var fca = CreateRecord(
            source: SourceType.Fca,
            companyNumber: "02922768",
            fcaFrn: "910922");

        var crawler = CreateRecord(
            source: SourceType.Crawler,
            companyNumber: "02922768",
            fcaFrn: "628633");

        var scorer = new MatchScorer();
        var result = scorer.Compare(fca, crawler);

        Assert.True(result.IsMatch);
    }

    [Fact]
    public void Compare_MatchingFcarWithConflictingCompanyNumber_DoesNotMatch()
    {
        var fca = CreateRecord(
            source: SourceType.Fca,
            companyNumber: "02922768",
            fcaFrn: "910922");

        var crawler = CreateRecord(
            source: SourceType.Crawler,
            companyNumber: "02933768",
            fcaFrn: "910922");

        var scorer = new MatchScorer();
        var result = scorer.Compare(fca, crawler);

        Assert.False(result.IsMatch);
    }

    // [Fact]
    // public void Compare_FcaTradingNameMatch_IsStrongMatch()
    // {
    //     var fca = CreateRecord(
    //         SourceType.Fca,
    //         "983390",
    //         tradingNames: ["WESTON VAN CENTRE"]);

    //     var marketCheck = CreateRecord(
    //         SourceType.MarketCheck,
    //         "MC945466",
    //         names: ["WESTON VAN CENTRE"]);

    //     var scorer = new MatchScorer();
    //     var result = scorer.Compare(
    //         fca,
    //         marketCheck);

    //     Assert.True(result.IsMatch);
    //     Assert.Equal(100, result.Score);
    // }

    [Fact]
    public void Compare_GenericNameMatch_Alone_IsNotMatch()
    {
        var left = CreateRecord(
            SourceType.MarketCheck,
            "MC-1",
            names: ["WESTON VAN CENTRE"]);

        var right = CreateRecord(
            SourceType.Crawler,
            "CRW-1",
            names: ["WESTON VAN CENTRE"]);

        var scorer = new MatchScorer();
        var result = scorer.Compare(
            left,
            right);

        Assert.False(result.IsMatch);
        Assert.Equal(40, result.Score);
    }

    [Fact]
    public void Compare_FcaLegalNameMatch_DoesNotGetTradingNameBonus()
    {
        var fca = CreateRecord(
            SourceType.Fca,
            "983390",
            legalName: "WESTON MOTOR GROUP LIMITED",
            names: ["WESTON MOTOR GROUP LIMITED"]);

        var other = CreateRecord(
            SourceType.Saf,
            "SAF-1",
            names: ["WESTON MOTOR GROUP LIMITED"]);

        var scorer = new MatchScorer();
        var result = scorer.Compare(
            fca,
            other);

        Assert.False(result.IsMatch);
        Assert.Equal(40, result.Score);
    }

    private static NormalizedRecord CreateRecord(SourceType? source = null,
    string? companyNumber = null,
    string? vatNumber = null,
    string? fcaFrn = null,
    string? domain = null,
    string? postcode = null,
    List<string>? names = null,
    List<string>? phones = null,
    List<string>? emails = null,
    string[]? tradingNames = null,
    string? legalName = null)
{
    return new NormalizedRecord
    {
        Source = source ?? SourceType.Crawler,
        SourceRecordId = Guid.NewGuid().ToString(),

        Names = names ?? [],
        TradingNames = tradingNames?.ToList() ?? [],
        LegalName = legalName,

        CompanyNumber = companyNumber,
        VatNumber = vatNumber,
        FcaFrn = fcaFrn,

        Address = postcode is null
            ? null
            : new NormalizedAddress
            {
                Postcode = postcode
            },

        Phones = phones ?? [],
        Emails = emails ?? [],

        Domain = domain
    };
}
}