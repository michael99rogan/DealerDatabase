using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class NameNormalizerTests
{
    [Theory]
    [InlineData(" Carver & Radcliffe  Limited", "CARVER & RADCLIFFE LIMITED")]
    [InlineData("Thornley & Pickering", "THORNLEY & PICKERING")]
    [InlineData(" REGAL CAR SUPERMARKET ", "REGAL CAR SUPERMARKET")]
    public void Normalize_ReturnsExpectedValue(
        string input,
        string expected)
    {
        var result = NameNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_WhenValueIsEmpty_ReturnsNull(
        string? input)
    {
        var result = NameNormalizer.Normalize(input);

        Assert.Null(result);
    }
}