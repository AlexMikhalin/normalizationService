namespace NormalizationService.Api.Sources;

public sealed class TaxDeskExpense
{
    public required string ReceiptCode { get; init; }

    public DateOnly PurchaseDate { get; init; }

    public required string VendorLegalName { get; init; }

    public decimal AmountIncludingVat { get; init; }

    public decimal VatAmount { get; init; }

    public required string CurrencyIso { get; init; }

    public required string ExpenseCategoryCode { get; init; }

    public bool Deductible { get; init; }

    public required string Notes { get; init; }
}
