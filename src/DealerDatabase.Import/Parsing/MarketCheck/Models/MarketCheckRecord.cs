namespace DealerDatabase.Import.Parsing.MarketCheck.Models;

public class MarketCheckRecord
{
    public required string DealerId { get; set; }
    public string? SellerName { get; set; }
    public string? SellerType { get; set; }
    public string? FranchiseMake { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }

    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Email { get; set; }

    public int? InventoryCount { get; set; }
    public decimal? AverageListedPrice { get; set; }
    public decimal? AverageSoldPrice { get; set; }
    public int? AverageDaysInStock { get; set; }
    public int? SoldLast30Days { get; set; }

    public string? VehicleTypes { get; set; }
    public decimal? CarPercentage { get; set; }
    public decimal? VanPercentage { get; set; }

    public int? CountUnder80kMilesUnder7Years { get; set; }
    public int? CountUnder100kMilesUnder10Years { get; set; }
    public int? CountUnder120kMilesUnder12Years { get; set; }
    public int? CountOver20YearsOver50kMiles { get; set; }

    public int? CountPriceOver10k { get; set; }
    public int? CountPriceOver20k { get; set; }
    public int? CountPriceOver50k { get; set; }

    public string? StockFeedProvider { get; set; }
    public string? LastSeen { get; set; }
}