namespace DealerDatabase.Import.Parsing.Crawler.Models;

public class CrawlRecord
{
    // Crawl metadata
    public required string CrawlId { get; set; }
    public string? CrawledAt { get; set; }
    public string? SourceUrl { get; set; }
    public string? FinalUrl { get; set; }
    public int? HttpStatus { get; set; }
    public string? PageType { get; set; }

    // Page content
    public string? PageTitle { get; set; }
    public string? H1 { get; set; }
    public string? MetaDescription { get; set; }

    // Detected dealer information
    public string? BusinessNameDetected { get; set; }
    public string? AddressDetected { get; set; }
    public string? PostcodeDetected { get; set; }
    public string? PhonesDetected { get; set; }
    public string? EmailsDetected { get; set; }

    public string? FooterText { get; set; }

    public string? CompanyNumberDetected { get; set; }
    public string? VatNumberDetected { get; set; }
    public string? FcaFrnDetected { get; set; }

    // Finance / website information
    public string? FinanceCalculator { get; set; }
    public string? FinanceLendersDetected { get; set; }
    public string? RepresentativeApr { get; set; }

    public string? WebsitePlatform { get; set; }
    public string? CmsVersion { get; set; }

    // Ratings / social media
    public string? GoogleRating { get; set; }
    public string? GoogleReviewCount { get; set; }
    public string? TrustpilotScore { get; set; }

    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }

    // Dealer information
    public string? OpeningHoursRaw { get; set; }
    public string? StockCountDetected { get; set; }
    public string? PartExchange { get; set; }
    public string? WarrantyOffered { get; set; }

    // Technical crawl information
    public string? CookieBanner { get; set; }
    public int? WordCount { get; set; }
    public int? ResponseMs { get; set; }
    public string? ContentHash { get; set; }
    public string? Lang { get; set; }
}