using CsvHelper.Configuration;
using DealerDatabase.Import.Parsing.MarketCheck.Models;

namespace DealerDatabase.Import.Parsing.MarketCheck.Models;

public sealed class MarketCheckRecordMap : ClassMap<MarketCheckRecord>
{
    public MarketCheckRecordMap()
    {
        Map(m => m.DealerId).Name("mc_dealer_id");
        Map(m => m.SellerName).Name("seller_name");
        Map(m => m.SellerType).Name("seller_type");
        Map(m => m.FranchiseMake).Name("franchise_make");

        Map(m => m.Street).Name("street");
        Map(m => m.City).Name("city");
        Map(m => m.County).Name("county");
        Map(m => m.Postcode).Name("postcode");

        Map(m => m.Phone).Name("phone");
        Map(m => m.Website).Name("website");
        Map(m => m.Email).Name("email");

        Map(m => m.InventoryCount).Name("inventory_count");
        Map(m => m.AverageListedPrice).Name("avg_listed_price");
        Map(m => m.AverageSoldPrice).Name("avg_sold_price");
        Map(m => m.AverageDaysInStock).Name("avg_days_in_stock");
        Map(m => m.SoldLast30Days).Name("sold_last_30_days");

        Map(m => m.VehicleTypes).Name("vehicle_types");
        Map(m => m.CarPercentage).Name("car_pct");
        Map(m => m.VanPercentage).Name("van_pct");

        Map(m => m.CountUnder80kMilesUnder7Years)
            .Name("cnt_under_80k_miles_under_7yrs");

        Map(m => m.CountUnder100kMilesUnder10Years)
            .Name("cnt_under_100k_miles_under_10yrs");

        Map(m => m.CountUnder120kMilesUnder12Years)
            .Name("cnt_under_120k_miles_under_12yrs");

        Map(m => m.CountOver20YearsOver50kMiles)
            .Name("cnt_over_20yrs_over_50k_miles");

        Map(m => m.CountPriceOver10k).Name("cnt_price_over_10k");
        Map(m => m.CountPriceOver20k).Name("cnt_price_over_20k");
        Map(m => m.CountPriceOver50k).Name("cnt_price_over_50k");

        Map(m => m.StockFeedProvider).Name("stock_feed_provider");
        Map(m => m.LastSeen).Name("last_seen");
    }
}