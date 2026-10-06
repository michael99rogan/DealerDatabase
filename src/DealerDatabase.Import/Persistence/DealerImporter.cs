using DealerDatabase.Data;
using DealerDatabase.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DealerDatabase.Import.Persistence;

public class DealerImporter(
    DealerDbContext dbContext)
{
    public async Task<int> ReplaceAllAsync(
    IEnumerable<Dealer> dealers)
    {
        await using var transaction =
            await dbContext.Database.BeginTransactionAsync();

        await dbContext.Dealers.ExecuteDeleteAsync();

        dbContext.Dealers.AddRange(dealers);

        await dbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        return await dbContext.Dealers.CountAsync();
    }
}