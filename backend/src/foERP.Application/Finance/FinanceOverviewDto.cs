namespace foERP.Application.Finance;

public sealed record FinanceOverviewDto(
    int LedgerAccountCount,
    decimal TrialBalanceAmount,
    DateTime SnapshotAtUtc);
