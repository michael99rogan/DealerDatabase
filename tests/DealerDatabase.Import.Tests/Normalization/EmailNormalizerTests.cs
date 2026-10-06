using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class EmailNormalizerTests
{
    [Theory]
    [InlineData(
        "Sales@FairbrotherCarSales.com",
        "sales@fairbrothercarsales.com")]
    [InlineData(
        " enquiries@example.com ",
        "enquiries@example.com")]
    public void Normalize_ReturnsExpectedValue(
        string input,
        string expected)
    {
        var result = EmailNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_WhenValueIsEmpty_ReturnsNull(
        string? input)
    {
        var result = EmailNormalizer.Normalize(input);

        Assert.Null(result);
    }
}