namespace DealerDatabase.Import.Normalization.Common;

public static class AddressNormalizer
{
    public static string? NormalizePart(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return string.Join(
            ' ',
            value
                .Trim()
                .Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries))
            .ToUpperInvariant();
    }
}