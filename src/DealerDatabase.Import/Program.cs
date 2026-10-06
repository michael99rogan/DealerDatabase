using DealerDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DealerDatabase.Import.Parsing.CompaniesHouse;
using DealerDatabase.Import.Parsing.Fca;
using DealerDatabase.Import.Parsing.MarketCheck;
using DealerDatabase.Import.Parsing.Ico;
using DealerDatabase.Import.Parsing.Vat;
using DealerDatabase.Import.Parsing.Crawler;
using DealerDatabase.Import.Parsing.Saf;
using DealerDatabase.Import.Normalization.Models;
using DealerDatabase.Import.Normalization.Sources;
using DealerDatabase.Import.Matching;
using DealerDatabase.Import.Consolidation;
using DealerDatabase.Import.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDealerDatabase();
builder.Services.AddScoped<DealerImporter>();

using var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

await using (var scope = host.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DealerDbContext>();
    await db.Database.MigrateAsync();
}

logger.LogInformation("Database: {DatabaseFile}", SolutionPaths.DatabaseFile);
logger.LogInformation("Data folder: {DataDirectory}", SolutionPaths.DataDirectory);

foreach (var entry in Directory.EnumerateFileSystemEntries(SolutionPaths.DataDirectory).Order())
{
    logger.LogInformation("  Found source: {Name}", Path.GetFileName(entry));
}

// Parsing
var companiesHousePath = Path.Combine(
    SolutionPaths.DataDirectory,
    "companies_house.json"
);
var companiesHouseParser = new CompaniesHouseParser();
var companiesHouseImport = companiesHouseParser.Parse(companiesHousePath);

var fcaPath = Path.Combine(
    SolutionPaths.DataDirectory,
    "fca_register.json"
);
var fcaParser = new FcaParser();
var fcaImport = fcaParser.Parse(fcaPath);

var marketCheckPath = Path.Combine(
    SolutionPaths.DataDirectory,
    "marketcheck_dealers.csv"
);
var marketCheckParser = new MarketCheckParser();
var marketCheckImport = marketCheckParser.Parse(marketCheckPath);

var icoPath = Path.Combine(
    SolutionPaths.DataDirectory,
    "ico_register.csv"
);
var icoParser = new IcoParser();
var icoImport = icoParser.Parse(icoPath);

var vatPath = Path.Combine(
    SolutionPaths.DataDirectory,
    "vat_lookups"
);
var vatParser = new VatParser();
var vatImport = vatParser.Parse(vatPath);

var crawlerPath = Path.Combine(
    SolutionPaths.DataDirectory,
    "crawled_dealers.csv"
);
var crawlerParser = new CrawlerParser();
var crawlerImport = crawlerParser.Parse(crawlerPath);

var safPath = Path.Combine(
    SolutionPaths.DataDirectory,
    "saf_members.xml"
);
var safParser = new SafParser();
var safImport = safParser.Parse(safPath);

// Normalization
var companiesHouseNormalizer =
    new CompaniesHouseNormalizer();

var fcaNormalizer =
    new FcaNormalizer();

var marketCheckNormalizer =
    new MarketCheckNormalizer();

var icoNormalizer =
    new IcoNormalizer();

var safNormalizer =
    new SafNormalizer();

var vatNormalizer =
    new VatNormalizer();

var crawlerNormalizer =
    new CrawlerNormalizer();

var normalizedRecords =
    new List<NormalizedRecord>();

normalizedRecords.AddRange(
    companiesHouseImport.Items.Select(
        companiesHouseNormalizer.Normalize
    )
);

normalizedRecords.AddRange(
    fcaImport.Data.Select(
        fcaNormalizer.Normalize
    )
);

normalizedRecords.AddRange(
    marketCheckImport.Select(
        marketCheckNormalizer.Normalize
    )
);

normalizedRecords.AddRange(
    icoImport.Select(
        icoNormalizer.Normalize
    )
);

normalizedRecords.AddRange(
    safImport.Members.Select(
        safNormalizer.Normalize
    )
);

// Separate logic for VAT as null returns possible
foreach (var record in vatImport)
{
    var normalized = vatNormalizer.Normalize(record);

    if (normalized is not null)
    {
        normalizedRecords.Add(normalized);
    }
}

normalizedRecords.AddRange(
    crawlerImport.Select(
        crawlerNormalizer.Normalize
    )
);

logger.LogInformation(
    "Total normalized records: {Count}",
    normalizedRecords.Count);

foreach (var group in normalizedRecords.GroupBy(x => x.Source))
{
    logger.LogInformation(
        "{Source}: {Count} normalized records",
        group.Key,
        group.Count());
}

// Grouped Matching
var matchScorer = new MatchScorer();
var recordMatcher = new RecordMatcher(matchScorer);

var matchedGroups = recordMatcher.Match(normalizedRecords);

logger.LogInformation(
    "Matched groups: {Count}",
    matchedGroups.Count);

logger.LogInformation(
    "Singleton groups: {Count}",
    matchedGroups.Count(group => group.Records.Count == 1));

logger.LogInformation(
    "Multi-record groups: {Count}",
    matchedGroups.Count(group => group.Records.Count > 1));

// Validation
var validator = new GroupValidator();

var validationResults = matchedGroups
    .Select(validator.Validate)
    .ToList();

logger.LogInformation(
    "Validation conflicts: {Count}",
    validationResults.Count(result =>
        result.HasConflict));


// Consolidation
var consolidator = new DealerConsolidator();

var consolidationResults = validationResults
    .Where (result => !result.HasConflict)
    .Select(result =>
        consolidator.Consolidate(result.Group))
    .ToList();

var dealers = consolidationResults
    .Select(result => result.Dealer)
    .ToList();

logger.LogInformation(
    "Consolidated dealers: {Count}",
    dealers.Count);

await using (var scope =
    host.Services.CreateAsyncScope())
{
    var importer =
        scope.ServiceProvider
            .GetRequiredService<DealerImporter>();

    var persistedCount =
        await importer.ReplaceAllAsync(dealers);

    logger.LogInformation(
        "Persisted dealers: {Count}",
        persistedCount);
}