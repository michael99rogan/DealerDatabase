namespace DealerDatabase.Import.Normalization.Common;

public static class PhoneNormalizer
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var digits = new string(
            value.Where(char.IsDigit).ToArray());

        if (digits.StartsWith("0044"))
        {
            return "44" + digits[4..];
        }

        if (digits.StartsWith("44"))
        {
            return digits;
        }

        if (digits.StartsWith("0"))
        {
            return "44" + digits[1..];
        }

        return digits;
    }

    // Helper for returning multiple names at once
    // Useful as some records have multiple names like legal name, trading name, etc.
    public static IEnumerable<string> NormalizeMany(
    params string?[] values)
    {
        return values
            .Select(Normalize)
            .Where(x => x is not null)
            .Distinct()
            .Cast<string>();
    }
}