using CsvHelper.Configuration;

namespace DealerDatabase.Import.Parsing.Crawler.Models;

public sealed class CrawlRecordMap : ClassMap<CrawlRecord>
{
    public CrawlRecordMap()
    {
        Map(m => m.CrawlId).Name("crawl_id");
        Map(m => m.CrawledAt).Name("crawled_at");
        Map(m => m.SourceUrl).Name("source_url");
        Map(m => m.FinalUrl).Name("final_url");
        Map(m => m.HttpStatus).Name("http_status");
        Map(m => m.PageType).Name("page_type");

        Map(m => m.PageTitle).Name("page_title");
        Map(m => m.H1).Name("h1");
        Map(m => m.MetaDescription).Name("meta_description");

        Map(m => m.BusinessNameDetected)
            .Name("business_name_detected");

        Map(m => m.AddressDetected)
            .Name("address_detected");

        Map(m => m.PostcodeDetected)
            .Name("postcode_detected");

        Map(m => m.PhonesDetected)
            .Name("phones_detected");

        Map(m => m.EmailsDetected)
            .Name("emails_detected");

        Map(m => m.FooterText)
            .Name("footer_text");

        Map(m => m.CompanyNumberDetected)
            .Name("company_number_detected");

        Map(m => m.VatNumberDetected)
            .Name("vat_number_detected");

        Map(m => m.FcaFrnDetected)
            .Name("fca_frn_detected");

        Map(m => m.FinanceCalculator)
            .Name("finance_calculator");

        Map(m => m.FinanceLendersDetected)
            .Name("finance_lenders_detected");

        Map(m => m.RepresentativeApr)
            .Name("representative_apr");

        Map(m => m.WebsitePlatform)
            .Name("website_platform");

        Map(m => m.CmsVersion)
            .Name("cms_version");

        Map(m => m.GoogleRating)
            .Name("google_rating");

        Map(m => m.GoogleReviewCount)
            .Name("google_review_count");

        Map(m => m.TrustpilotScore)
            .Name("trustpilot_score");

        Map(m => m.FacebookUrl)
            .Name("facebook_url");

        Map(m => m.InstagramUrl)
            .Name("instagram_url");

        Map(m => m.OpeningHoursRaw)
            .Name("opening_hours_raw");

        Map(m => m.StockCountDetected)
            .Name("stock_count_detected");

        Map(m => m.PartExchange)
            .Name("part_exchange");

        Map(m => m.WarrantyOffered)
            .Name("warranty_offered");

        Map(m => m.CookieBanner)
            .Name("cookie_banner");

        Map(m => m.WordCount)
            .Name("word_count");

        Map(m => m.ResponseMs)
            .Name("response_ms");

        Map(m => m.ContentHash)
            .Name("content_hash");

        Map(m => m.Lang)
            .Name("lang");
    }
}