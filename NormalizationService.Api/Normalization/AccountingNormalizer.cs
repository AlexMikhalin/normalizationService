using NormalizationService.Api.Domain;
using NormalizationService.Api.Sources;

namespace NormalizationService.Api.Normalization;

public sealed class AccountingNormalizer(IAccountingFeedReader feedReader) : IAccountingNormalizer
{
    public async Task<IReadOnlyCollection<NormalizedAccountingEntry>> NormalizeAsync(
        CancellationToken cancellationToken = default)
    {
        var feed = await feedReader.ReadAsync(cancellationToken);

        return feed.QuickLedgerInvoices.Select(NormalizeQuickLedgerInvoice)
            .Concat(feed.CloudBooksJournalEntries.Select(NormalizeCloudBooksJournalEntry))
            .Concat(feed.PayFlowSettlements.Select(NormalizePayFlowSettlement))
            .Concat(feed.TaxDeskExpenses.Select(NormalizeTaxDeskExpense))
            .Concat(feed.BankBridgeStatementLines.Select(NormalizeBankBridgeStatementLine))
            .ToArray();
    }

    private static NormalizedAccountingEntry NormalizeQuickLedgerInvoice(QuickLedgerInvoice invoice)
    {
        return new NormalizedAccountingEntry
        {
            SourceSystem = "QuickLedger",
            ExternalId = invoice.InvoiceId,
            EntryType = "SalesInvoice",
            OccurredAt = invoice.IssuedAt,
            AccountCode = invoice.RevenueAccount,
            CounterpartyName = invoice.CustomerName,
            Currency = invoice.CurrencyCode,
            DebitAmount = 0,
            CreditAmount = invoice.TotalDue,
            NetAmount = invoice.Subtotal,
            TaxAmount = invoice.SalesTax,
            GrossAmount = invoice.TotalDue,
            Description = invoice.Memo,
            Status = invoice.PaymentState,
            ReferenceNumber = invoice.InvoiceId
        };
    }

    private static NormalizedAccountingEntry NormalizeCloudBooksJournalEntry(CloudBooksJournalEntry journalEntry)
    {
        var amount = journalEntry.Debit > 0 ? journalEntry.Debit : journalEntry.Credit;

        return new NormalizedAccountingEntry
        {
            SourceSystem = "CloudBooks",
            ExternalId = journalEntry.EntryNumber,
            EntryType = "JournalEntry",
            OccurredAt = journalEntry.PostedOn.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            AccountCode = journalEntry.LedgerAccount,
            CounterpartyName = journalEntry.Counterparty,
            Currency = journalEntry.IsoCurrency,
            DebitAmount = journalEntry.Debit,
            CreditAmount = journalEntry.Credit,
            NetAmount = amount,
            TaxAmount = 0,
            GrossAmount = amount,
            Description = journalEntry.Narrative,
            Status = journalEntry.Approved ? "Approved" : "Draft",
            ReferenceNumber = journalEntry.EntryNumber
        };
    }

    private static NormalizedAccountingEntry NormalizePayFlowSettlement(PayFlowSettlement settlement)
    {
        return new NormalizedAccountingEntry
        {
            SourceSystem = "PayFlow",
            ExternalId = settlement.SettlementReference,
            EntryType = "PaymentSettlement",
            OccurredAt = settlement.CompletedAt,
            AccountCode = settlement.ClearingAccount,
            CounterpartyName = settlement.Merchant,
            Currency = settlement.Currency,
            DebitAmount = settlement.Net,
            CreditAmount = 0,
            NetAmount = settlement.Net,
            TaxAmount = settlement.TaxWithheld,
            GrossAmount = settlement.Gross,
            Description = $"Gross {settlement.Gross}, fee {settlement.Fee}",
            Status = settlement.StatusCode,
            ReferenceNumber = settlement.SettlementReference
        };
    }

    private static NormalizedAccountingEntry NormalizeTaxDeskExpense(TaxDeskExpense expense)
    {
        return new NormalizedAccountingEntry
        {
            SourceSystem = "TaxDesk",
            ExternalId = expense.ReceiptCode,
            EntryType = "VendorExpense",
            OccurredAt = expense.PurchaseDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            AccountCode = expense.ExpenseCategoryCode,
            CounterpartyName = expense.VendorLegalName,
            Currency = expense.CurrencyIso,
            DebitAmount = expense.AmountIncludingVat,
            CreditAmount = 0,
            NetAmount = expense.AmountIncludingVat - expense.VatAmount,
            TaxAmount = expense.VatAmount,
            GrossAmount = expense.AmountIncludingVat,
            Description = expense.Notes,
            Status = expense.Deductible ? "Deductible" : "NonDeductible",
            ReferenceNumber = expense.ReceiptCode
        };
    }

    private static NormalizedAccountingEntry NormalizeBankBridgeStatementLine(BankBridgeStatementLine statementLine)
    {
        var debitAmount = statementLine.Direction.Equals("Credit", StringComparison.OrdinalIgnoreCase)
            ? statementLine.Amount
            : 0;
        var creditAmount = statementLine.Direction.Equals("Debit", StringComparison.OrdinalIgnoreCase)
            ? statementLine.Amount
            : 0;

        return new NormalizedAccountingEntry
        {
            SourceSystem = "BankBridge",
            ExternalId = statementLine.BankTxnId,
            EntryType = "BankStatementLine",
            OccurredAt = statementLine.BookingDate,
            AccountCode = $"BANK-{statementLine.AccountIbanLast4}",
            CounterpartyName = statementLine.CounterpartyDisplay,
            Currency = statementLine.CurrencyAlpha,
            DebitAmount = debitAmount,
            CreditAmount = creditAmount,
            NetAmount = statementLine.Amount,
            TaxAmount = 0,
            GrossAmount = statementLine.Amount,
            Description = statementLine.DescriptionLine,
            Status = string.IsNullOrWhiteSpace(statementLine.MatchedDocument) ? "Unmatched" : "Matched",
            ReferenceNumber = statementLine.MatchedDocument
        };
    }
}
