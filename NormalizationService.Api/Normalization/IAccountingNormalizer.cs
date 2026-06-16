using NormalizationService.Api.Domain;

namespace NormalizationService.Api.Normalization;

public interface IAccountingNormalizer
{
    Task<IReadOnlyCollection<NormalizedAccountingEntry>> NormalizeAsync(
        CancellationToken cancellationToken = default);
}
