using NormalizationService.Api.Domain;

namespace NormalizationService.Api.Application;

public sealed class NormalizedAccountingEntryQueryService(
    INormalizedAccountingEntryRepository repository)
    : INormalizedAccountingEntryQueryService
{
    public Task<IReadOnlyCollection<NormalizedAccountingEntry>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return repository.GetAllAsync(cancellationToken);
    }
}
