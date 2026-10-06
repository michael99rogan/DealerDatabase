using DealerDatabase.Import.Normalization.Common;

namespace DealerDatabase.Import.Tests.Normalization;

public class DomainNormalizerTests
{
    [Theory]
    [InlineData(
        "https://www.fairbrothercarsales.com/",
        "fairbrothercarsales.com")]
    [InlineData(
        "http://fairbrothercarsales.com/finance",
        "fairbrothercarsales.com")]
    [InlineData(
        "https://fairbrothercarsales.com/contact-us",
        "fairbrothercarsales.com")]
    [InlineData(
        "www.fairbrothercarsales.com",
        "fairbrothercarsales.com")]
    [InlineData(
        "fairbrothercarsales.com",
        "fairbrothercarsales.com")]
    [InlineData(
        "HTTPS://WWW.FAIRBROTHERCARSALES.COM/",
        "fairbrothercarsales.com")]
    [InlineData(
        "  https://www.fairbrothercarsales.com/  ",
        "fairbrothercarsales.com")]
    public void Normalize_ValidDomain_ReturnsNormalizedDomain(
        string input,
        string expected)
    {
        var result = DomainNormalizer.Normalize(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_EmptyValue_ReturnsNull(string? input)
    {
        var result = DomainNormalizer.Normalize(input);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("not a valid domain")]
    [InlineData("://broken")]
    public void Normalize_InvalidDomain_ReturnsNull(string input)
    {
        var result = DomainNormalizer.Normalize(input);

        Assert.Null(result);
    }
}