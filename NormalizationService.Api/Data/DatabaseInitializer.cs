using Microsoft.EntityFrameworkCore;
using NormalizationService.Api.Normalization;

namespace NormalizationService.Api.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AccountingDbContext>();
        var normalizer = scope.ServiceProvider.GetRequiredService<IAccountingNormalizer>();

        await dbContext.Database.EnsureCreatedAsync();

        var hasEntries = await dbContext.NormalizedAccountingEntries.AnyAsync();
        if (hasEntries)
        {
            return;
        }

        var entries = await normalizer.NormalizeAsync();

        dbContext.NormalizedAccountingEntries.AddRange(entries);
        await dbContext.SaveChangesAsync();
    }
}
