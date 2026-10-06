using System.Globalization;

namespace DealerDatabase.Import.Normalization.Common;

public static class DateNormalizer
{
    public static DateOnly? NormalizeDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string[] formats =
        [
            "yyyy-MM-dd",
            "dd/MM/yyyy",
            "d MMM yyyy",
            "d MMMM yyyy"
        ];

        if (DateOnly.TryParseExact(
                value.Trim(),
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            return date;
        }

        return null;
    }
}