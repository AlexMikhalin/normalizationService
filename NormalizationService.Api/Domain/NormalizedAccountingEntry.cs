namespace NormalizationService.Api.Domain;

public sealed class NormalizedAccountingEntry
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string SourceSystem { get; init; }

    public required string ExternalId { get; init; }

    public required string EntryType { get; init; }

    public DateTimeOffset OccurredAt { get; init; }

    public required string AccountCode { get; init; }

    public required string CounterpartyName { get; init; }

    public required string Currency { get; init; }

    public decimal DebitAmount { get; init; }

    public decimal CreditAmount { get; init; }

    public decimal NetAmount { get; init; }

    public decimal TaxAmount { get; init; }

    public decimal GrossAmount { get; init; }

    public required string Description { get; init; }

    public required string Status { get; init; }

    public required string ReferenceNumber { get; init; }

    public DateTimeOffset IngestedAtUtc { get; init; } = DateTimeOffset.UtcNow;
}
