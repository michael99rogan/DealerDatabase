using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class DateNormalizerTests
{
    [Theory]
    [InlineData("2027-01-31", 2027, 1, 31)]
    [InlineData("22/12/2025", 2025, 12, 22)]
    [InlineData("  16 Aug 2026 ", 2026, 8, 16)]
    [InlineData("5 March 2027 ", 2027, 3, 5)]
    [InlineData("31/05/2026", 2026, 5, 31)]
    public void NormalizeDate_SupportedFormat_ReturnsDate(
        string input,
        int year,
        int month,
        int day)
    {
        var result = DateNormalizer.NormalizeDate(input);

        Assert.Equal(
            new DateOnly(year, month, day),
            result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NormalizeDate_EmptyValue_ReturnsNull(string? input)
    {
        var result = DateNormalizer.NormalizeDate(input);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("not a date")]
    [InlineData("31/31/2026")]
    [InlineData("2026-99-99")]
    public void NormalizeDate_InvalidValue_ReturnsNull(string input)
    {
        var result = DateNormalizer.NormalizeDate(input);

        Assert.Null(result);
    }
}