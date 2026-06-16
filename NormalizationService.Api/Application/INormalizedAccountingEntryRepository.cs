using NormalizationService.Api.Domain;

namespace NormalizationService.Api.Application;

public interface INormalizedAccountingEntryRepository
{
    Task<IReadOnlyCollection<NormalizedAccountingEntry>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
