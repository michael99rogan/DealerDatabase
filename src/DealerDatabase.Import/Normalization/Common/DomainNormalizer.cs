namespace DealerDatabase.Import.Normalization.Common;

public static class DomainNormalizer
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var candidate = value.Trim();

        if (!candidate.Contains("://"))
        {
            candidate = $"https://{candidate}";
        }

        if (!Uri.TryCreate(
                candidate,
                UriKind.Absolute,
                out var uri))
        {
            return null;
        }

        var host = uri.Host.ToLowerInvariant();

        if (host.StartsWith("www."))
        {
            host = host[4..];
        }

        return host;
    }
}