using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class CompanyNumberNormalizerTests
{
    [Theory]
    [InlineData("05860635", "05860635")]
    [InlineData("6028865", "06028865")]
    [InlineData(" SC117044 ", "SC117044")]
    public void Normalize_ReturnsExpectedValue(
        string input,
        string expected)
    {
        var result = CompanyNumberNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_WhenValueIsEmpty_ReturnsNull(
        string? input)
    {
        var result = CompanyNumberNormalizer.Normalize(input);

        Assert.Null(result);
    }
}