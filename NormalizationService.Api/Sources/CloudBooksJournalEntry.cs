namespace NormalizationService.Api.Sources;

public sealed class CloudBooksJournalEntry
{
    public required string EntryNumber { get; init; }

    public DateOnly PostedOn { get; init; }

    public required string LedgerAccount { get; init; }

    public decimal Debit { get; init; }

    public decimal Credit { get; init; }

    public required string Counterparty { get; init; }

    public required string IsoCurrency { get; init; }

    public required string Narrative { get; init; }

    public bool Approved { get; init; }
}
