namespace DealerDatabase.Import.Normalization.Common;

public static class EmailNormalizer
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim().ToLowerInvariant();
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