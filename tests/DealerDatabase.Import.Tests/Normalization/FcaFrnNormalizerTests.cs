using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class FcaFrnNormalizerTests
{
    [Theory]
    [InlineData("938657", "938657")]
    [InlineData(" 938657 ", "938657")]
    public void Normalize_ReturnsExpectedValue(
        string input,
        string expected)
    {
        var result = FcaFrnNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_WhenValueIsEmpty_ReturnsNull(
        string? input)
    {
        var result = FcaFrnNormalizer.Normalize(input);

        Assert.Null(result);
    }
}