using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class PostcodeNormalizerTests
{
    [Theory]
    [InlineData("L20 8YE", "L208YE")]
    [InlineData("L208YE", "L208YE")]
    [InlineData(" b44 8jj ", "B448JJ")]
    [InlineData("cf104qd", "CF104QD")]
    [InlineData("CW17RL", "CW17RL")]
    public void Normalize_ReturnsExpectedValue(
        string input,
        string expected)
    {
        var result = PostcodeNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_WhenValueIsEmpty_ReturnsNull(
        string? input)
    {
        var result = PostcodeNormalizer.Normalize(input);

        Assert.Null(result);
    }
}