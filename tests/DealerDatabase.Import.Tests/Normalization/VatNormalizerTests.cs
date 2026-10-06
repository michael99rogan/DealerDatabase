using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class VatNormalizerTests
{
    [Theory]
    [InlineData("GB197604333", "197604333")]
    [InlineData("GB 197 6043 33", "197604333")]
    [InlineData("197604333", "197604333")]
    public void Normalize_ReturnsExpectedValue(
        string input,
        string expected)
    {
        var result = VatNumberNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_WhenValueIsEmpty_ReturnsNull(
        string? input)
    {
        var result = VatNumberNormalizer.Normalize(input);

        Assert.Null(result);
    }
}