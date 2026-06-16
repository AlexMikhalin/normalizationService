namespace NormalizationService.Api.Sources;

public sealed class AccountingFeed
{
    public List<QuickLedgerInvoice> QuickLedgerInvoices { get; init; } = [];

    public List<CloudBooksJournalEntry> CloudBooksJournalEntries { get; init; } = [];

    public List<PayFlowSettlement> PayFlowSettlements { get; init; } = [];

    public List<TaxDeskExpense> TaxDeskExpenses { get; init; } = [];

    public List<BankBridgeStatementLine> BankBridgeStatementLines { get; init; } = [];
}
