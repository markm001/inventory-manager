namespace InventoryManager.Core.Models;

public sealed record ItemStack(InventoryItem Item, long Amount) : IInventoryEntry
{
    public string Id => Item.Id;
}