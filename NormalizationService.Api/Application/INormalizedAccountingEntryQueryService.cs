using NormalizationService.Api.Domain;

namespace NormalizationService.Api.Application;

public interface INormalizedAccountingEntryQueryService
{
    Task<IReadOnlyCollection<NormalizedAccountingEntry>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
