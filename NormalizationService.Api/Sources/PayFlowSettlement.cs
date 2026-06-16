namespace NormalizationService.Api.Sources;

public sealed class PayFlowSettlement
{
    public required string SettlementReference { get; init; }

    public DateTimeOffset CompletedAt { get; init; }

    public required string Merchant { get; init; }

    public required string Currency { get; init; }

    public decimal Gross { get; init; }

    public decimal Fee { get; init; }

    public decimal Net { get; init; }

    public decimal TaxWithheld { get; init; }

    public required string ClearingAccount { get; init; }

    public required string StatusCode { get; init; }
}
