using Microsoft.EntityFrameworkCore;
using NormalizationService.Api.Contracts;
using NormalizationService.Api.Data;
using NormalizationService.Api.Normalization;
using NormalizationService.Api.Sources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AccountingDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("AccountingDatabase");
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<IAccountingFeedReader, JsonAccountingFeedReader>();
builder.Services.AddSingleton<IAccountingNormalizer, AccountingNormalizer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

await DatabaseInitializer.InitializeAsync(app.Services);

app.MapGet("/api/normalized-entries", async (
        AccountingDbContext dbContext,
        CancellationToken cancellationToken) =>
    {
        var entries = await dbContext.NormalizedAccountingEntries
            .AsNoTracking()
            .OrderBy(entry => entry.OccurredAt)
            .ThenBy(entry => entry.SourceSystem)
            .ThenBy(entry => entry.ExternalId)
            .Select(entry => NormalizedAccountingEntryResponse.FromEntity(entry))
            .ToListAsync(cancellationToken);

        return Results.Ok(entries);
    })
    .WithName("GetNormalizedAccountingEntries")
    .Produces<IReadOnlyCollection<NormalizedAccountingEntryResponse>>();

app.Run();
