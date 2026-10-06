namespace DealerDatabase.Import.Normalization.Common;

public static class CompanyNumberNormalizer
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

        // Padding left with 0 for numeric values
        // Done because crawler omits the 0 that is present on comapnies house
        // Assuming it is supposed to be the same number
        if (normalized.All(char.IsDigit) &&
            normalized.Length < 8)
        {
            normalized = normalized.PadLeft(8, '0');
        }

        return normalized;
    }
}