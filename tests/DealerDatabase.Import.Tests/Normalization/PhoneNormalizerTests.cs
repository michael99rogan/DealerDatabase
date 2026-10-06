using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class PhoneNormalizerTests
{
    [Theory]
    [InlineData("01632 960529", "441632960529")]
    [InlineData("+44 1632 960529", "441632960529")]
    [InlineData("0044 1632 960529", "441632960529")]
    public void Normalize_ReturnsExpectedValue(
        string input,
        string expected)
    {
        var result = PhoneNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_WhenValueIsEmpty_ReturnsNull(
        string? input)
    {
        var result = PhoneNormalizer.Normalize(input);

        Assert.Null(result);
    }
}