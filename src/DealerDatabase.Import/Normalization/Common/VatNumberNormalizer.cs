namespace DealerDatabase.Import.Normalization.Common;

public static class VatNumberNormalizer
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = new string(
            value
                .Where(char.IsLetterOrDigit)
                .Select(char.ToUpperInvariant)
                .ToArray());

        if (normalized.StartsWith("GB"))
        {
            normalized = normalized[2..];
        }

        return normalized;
    }
}