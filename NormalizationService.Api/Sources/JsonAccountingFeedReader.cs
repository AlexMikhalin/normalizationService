using System.Text.Json;

namespace NormalizationService.Api.Sources;

public sealed class JsonAccountingFeedReader(IWebHostEnvironment environment) : IAccountingFeedReader
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<AccountingFeed> ReadAsync(CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(environment.ContentRootPath, "Data", "Mock", "accounting-feed.json");

        await using var stream = File.OpenRead(filePath);
        var feed = await JsonSerializer.DeserializeAsync<AccountingFeed>(
            stream,
            SerializerOptions,
            cancellationToken);

        return feed ?? new AccountingFeed();
    }
}
