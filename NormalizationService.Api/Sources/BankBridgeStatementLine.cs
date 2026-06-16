namespace NormalizationService.Api.Sources;

public sealed class BankBridgeStatementLine
{
    public required string BankTxnId { get; init; }

    public DateTimeOffset BookingDate { get; init; }

    public required string AccountIbanLast4 { get; init; }

    public required string DescriptionLine { get; init; }

    public decimal Amount { get; init; }

    public required string Direction { get; init; }

    public required string CurrencyAlpha { get; init; }

    public required string CounterpartyDisplay { get; init; }

    public required string MatchedDocument { get; init; }
}
