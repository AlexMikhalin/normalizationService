using NormalizationService.Api.Normalization;
using NormalizationService.Api.Sources;

namespace NormalizationService.Tests;

public sealed class AccountingNormalizerTests
{
    [Fact]
    public async Task NormalizeAsync_returns_entries_from_all_source_formats()
    {
        var feed = CreateAccountingFeed();
        var normalizer = new AccountingNormalizer(new StaticAccountingFeedReader(feed));

        var entries = await normalizer.NormalizeAsync();

        Assert.Equal(5, entries.Count);
        Assert.Contains(entries, entry => entry.SourceSystem == "QuickLedger");
        Assert.Contains(entries, entry => entry.SourceSystem == "CloudBooks");
        Assert.Contains(entries, entry => entry.SourceSystem == "PayFlow");
        Assert.Contains(entries, entry => entry.SourceSystem == "TaxDesk");
        Assert.Contains(entries, entry => entry.SourceSystem == "BankBridge");
    }

    [Fact]
    public async Task NormalizeAsync_maps_accounting_amounts_and_statuses()
    {
        var feed = CreateAccountingFeed();
        var normalizer = new AccountingNormalizer(new StaticAccountingFeedReader(feed));

        var entries = await normalizer.NormalizeAsync();

        var invoice = Assert.Single(entries, entry => entry.SourceSystem == "QuickLedger");
        Assert.Equal(1000m, invoice.NetAmount);
        Assert.Equal(80m, invoice.TaxAmount);
        Assert.Equal(1080m, invoice.CreditAmount);
        Assert.Equal("Paid", invoice.Status);

        var bankLine = Assert.Single(entries, entry => entry.SourceSystem == "BankBridge");
        Assert.Equal(620m, bankLine.DebitAmount);
        Assert.Equal(0m, bankLine.CreditAmount);
        Assert.Equal("Matched", bankLine.Status);
    }

    private static AccountingFeed CreateAccountingFeed()
    {
        return new AccountingFeed
        {
            QuickLedgerInvoices =
            [
                new QuickLedgerInvoice
                {
                    InvoiceId = "QL-001",
                    IssuedAt = DateTimeOffset.Parse("2026-04-01T10:00:00Z"),
                    CustomerName = "Northwind Labs",
                    CurrencyCode = "USD",
                    Subtotal = 1000m,
                    SalesTax = 80m,
                    TotalDue = 1080m,
                    RevenueAccount = "4000",
                    PaymentState = "Paid",
                    Memo = "Consulting"
                }
            ],
            CloudBooksJournalEntries =
            [
                new CloudBooksJournalEntry
                {
                    EntryNumber = "CB-001",
                    PostedOn = new DateOnly(2026, 4, 2),
                    LedgerAccount = "6100",
                    Debit = 200m,
                    Credit = 0m,
                    Counterparty = "Office Supply",
                    IsoCurrency = "USD",
                    Narrative = "Supplies",
                    Approved = true
                }
            ],
            PayFlowSettlements =
            [
                new PayFlowSettlement
                {
                    SettlementReference = "PF-001",
                    CompletedAt = DateTimeOffset.Parse("2026-04-03T11:00:00Z"),
                    Merchant = "Bright Market",
                    Currency = "EUR",
                    Gross = 500m,
                    Fee = 15m,
                    Net = 485m,
                    TaxWithheld = 0m,
                    ClearingAccount = "1010",
                    StatusCode = "Settled"
                }
            ],
            TaxDeskExpenses =
            [
                new TaxDeskExpense
                {
                    ReceiptCode = "TD-001",
                    PurchaseDate = new DateOnly(2026, 4, 4),
                    VendorLegalName = "Cloud Hosting Ltd",
                    AmountIncludingVat = 240m,
                    VatAmount = 40m,
                    CurrencyIso = "GBP",
                    ExpenseCategoryCode = "6400",
                    Deductible = true,
                    Notes = "Infrastructure"
                }
            ],
            BankBridgeStatementLines =
            [
                new BankBridgeStatementLine
                {
                    BankTxnId = "BB-001",
                    BookingDate = DateTimeOffset.Parse("2026-04-05T08:00:00Z"),
                    AccountIbanLast4 = "4021",
                    DescriptionLine = "Customer payment",
                    Amount = 620m,
                    Direction = "Credit",
                    CurrencyAlpha = "USD",
                    CounterpartyDisplay = "ACME Studio",
                    MatchedDocument = "QL-001"
                }
            ]
        };
    }

    private sealed class StaticAccountingFeedReader(AccountingFeed feed) : IAccountingFeedReader
    {
        public Task<AccountingFeed> ReadAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(feed);
        }
    }
}
