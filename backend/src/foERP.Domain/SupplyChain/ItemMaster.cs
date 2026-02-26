using foERP.Domain.Abstractions;

namespace foERP.Domain.SupplyChain;

public sealed class ItemMaster : Entity
{
    public required string ItemId { get; init; }
    public required string ItemName { get; init; }
    public string? InventoryUnit { get; private set; }

    public void SetInventoryUnit(string inventoryUnit)
    {
        InventoryUnit = inventoryUnit;
        MarkModified();
    }
}
