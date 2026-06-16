namespace NormalizationService.Api.Sources;

public interface IAccountingFeedReader
{
    Task<AccountingFeed> ReadAsync(CancellationToken cancellationToken = default);
}
