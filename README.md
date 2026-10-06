# Dealer Database Technical Task

This solution imports dealer information from the supplied data sources, normalizes and matches records referring to the same dealer, consolidates the results, and persists the resulting dealer data to a SQLite database.

The solution also contains a small ASP.NET Core MVC application for viewing the imported dealer records.

## Requirements

- .NET 8 SDK
- EF Core command-line tools

## Build

From the root directory of the repository:

```bash
dotnet restore
dotnet build

## Import

dotnet run --project src/DealerDatabase.Import

## Web

dotnet run --project src/DealerDatabase.Web

## Run unit tests

dotnet test