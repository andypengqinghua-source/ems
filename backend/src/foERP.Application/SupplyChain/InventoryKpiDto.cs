namespace foERP.Application.SupplyChain;

public sealed record InventoryKpiDto(
    int ActiveItems,
    decimal InventoryValue,
    DateTime SnapshotAtUtc);
