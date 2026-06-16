using Microsoft.EntityFrameworkCore;
using NormalizationService.Api.Application;
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
builder.Services.AddScoped<INormalizedAccountingEntryRepository, EfNormalizedAccountingEntryRepository>();
builder.Services.AddScoped<INormalizedAccountingEntryQueryService, NormalizedAccountingEntryQueryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

await DatabaseInitializer.InitializeAsync(app.Services);

app.MapGet("/api/normalized-entries", async (
        INormalizedAccountingEntryQueryService queryService,
        CancellationToken cancellationToken) =>
    {
        var entries = await queryService.GetAllAsync(cancellationToken);
        var response = entries.Select(NormalizedAccountingEntryResponse.FromEntity);

        return Results.Ok(response);
    })
    .WithName("GetNormalizedAccountingEntries")
    .Produces<IReadOnlyCollection<NormalizedAccountingEntryResponse>>();

app.Run();
