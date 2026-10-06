namespace DealerDatabase.Import.Normalization.Common;

public static class FcaFrnNormalizer
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new string(
            value.Where(char.IsDigit).ToArray());
    }
}