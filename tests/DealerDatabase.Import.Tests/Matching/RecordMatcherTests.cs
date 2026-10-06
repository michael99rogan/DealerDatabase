using DealerDatabase.Import.Matching;
using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Tests.Matching;

public class RecordMatcherTests
{
    private readonly MatchScorer _scorer = new();

    [Fact]
    public void Match_EmptyList_ReturnsNoGroups()
    {
        var matcher = new RecordMatcher(_scorer);

        var records = new List<NormalizedRecord>();

        var result = matcher.Match(records);

        Assert.Empty(result);
    }

    [Fact]
    public void Match_UnrelatedRecords_ReturnsSeparateGroups()
    {
        var matcher = new RecordMatcher(_scorer);

        var first = CreateRecord(
            "RECORD-1",
            companyNumber: "11111111");

        var second = CreateRecord(
            "RECORD-2",
            companyNumber: "22222222");

        var records = new List<NormalizedRecord>
        {
            first,
            second
        };

        var result = matcher.Match(records);

        Assert.Equal(2, result.Count);

        Assert.Contains(
            result,
            group => group.Records.Count == 1 &&
                     group.Records.Contains(first));

        Assert.Contains(
            result,
            group => group.Records.Count == 1 &&
                     group.Records.Contains(second));
    }

    [Fact]
    public void Match_DirectlyMatchingRecords_ReturnsSingleGroup()
    {
        var matcher = new RecordMatcher(_scorer);

        var first = CreateRecord(
            "RECORD-1",
            companyNumber: "12345678");

        var second = CreateRecord(
            "RECORD-2",
            companyNumber: "12345678");

        var records = new List<NormalizedRecord>
        {
            first,
            second
        };

        var result = matcher.Match(records);

        Assert.Single(result);

        var group = result.Single();

        Assert.Equal(2, group.Records.Count);
        Assert.Contains(first, group.Records);
        Assert.Contains(second, group.Records);
    }

    [Fact]
    public void Match_TransitivelyMatchingRecords_ReturnsSingleGroup()
    {
        var matcher = new RecordMatcher(_scorer);

        // A matches B through company number.
        var recordA = CreateRecord(
            "RECORD-A",
            companyNumber: "12345678");

        // B matches A through company number,
        // and C through VAT number.
        var recordB = CreateRecord(
            "RECORD-B",
            companyNumber: "12345678",
            vatNumber: "987654321");

        // C has no direct evidence connecting it to A.
        var recordC = CreateRecord(
            "RECORD-C",
            vatNumber: "987654321");

        var records = new List<NormalizedRecord>
        {
            recordA,
            recordB,
            recordC
        };

        var result = matcher.Match(records);

        Assert.Single(result);

        var group = result.Single();

        Assert.Equal(3, group.Records.Count);

        Assert.Contains(recordA, group.Records);
        Assert.Contains(recordB, group.Records);
        Assert.Contains(recordC, group.Records);
    }

    private static NormalizedRecord CreateRecord(
        string sourceRecordId,
        string? companyNumber = null,
        string? vatNumber = null,
        string? fcaFrn = null,
        string? domain = null,
        string? postcode = null,
        List<string>? names = null,
        List<string>? phones = null,
        List<string>? emails = null)
    {
        return new NormalizedRecord
        {
            Source = SourceType.Crawler,
            SourceRecordId = sourceRecordId,

            Names = names ?? [],

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