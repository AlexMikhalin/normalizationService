using Microsoft.EntityFrameworkCore;
using NormalizationService.Api.Application;
using NormalizationService.Api.Domain;

namespace NormalizationService.Api.Data;

public sealed class EfNormalizedAccountingEntryRepository(AccountingDbContext dbContext)
    : INormalizedAccountingEntryRepository
{
    public async Task<IReadOnlyCollection<NormalizedAccountingEntry>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.NormalizedAccountingEntries
            .AsNoTracking()
            .OrderBy(entry => entry.OccurredAt)
            .ThenBy(entry => entry.SourceSystem)
            .ThenBy(entry => entry.ExternalId)
            .ToListAsync(cancellationToken);
    }
}
