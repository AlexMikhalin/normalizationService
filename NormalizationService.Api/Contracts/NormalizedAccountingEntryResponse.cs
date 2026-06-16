using NormalizationService.Api.Domain;

namespace NormalizationService.Api.Contracts;

public sealed record NormalizedAccountingEntryResponse(
    Guid Id,
    string SourceSystem,
    string ExternalId,
    string EntryType,
    DateTimeOffset OccurredAt,
    string AccountCode,
    string CounterpartyName,
    string Currency,
    decimal DebitAmount,
    decimal CreditAmount,
    decimal NetAmount,
    decimal TaxAmount,
    decimal GrossAmount,
    string Description,
    string Status,
    string ReferenceNumber,
    DateTimeOffset IngestedAtUtc)
{
    public static NormalizedAccountingEntryResponse FromEntity(NormalizedAccountingEntry entry)
    {
        return new NormalizedAccountingEntryResponse(
            entry.Id,
            entry.SourceSystem,
            entry.ExternalId,
            entry.EntryType,
            entry.OccurredAt,
            entry.AccountCode,
            entry.CounterpartyName,
            entry.Currency,
            entry.DebitAmount,
            entry.CreditAmount,
            entry.NetAmount,
            entry.TaxAmount,
            entry.GrossAmount,
            entry.Description,
            entry.Status,
            entry.ReferenceNumber,
            entry.IngestedAtUtc);
    }
}
