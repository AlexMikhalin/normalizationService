namespace NormalizationService.Api.Sources;

public sealed class QuickLedgerInvoice
{
    public required string InvoiceId { get; init; }

    public DateTimeOffset IssuedAt { get; init; }

    public required string CustomerName { get; init; }

    public required string CurrencyCode { get; init; }

    public decimal Subtotal { get; init; }

    public decimal SalesTax { get; init; }

    public decimal TotalDue { get; init; }

    public required string RevenueAccount { get; init; }

    public required string PaymentState { get; init; }

    public required string Memo { get; init; }
}
