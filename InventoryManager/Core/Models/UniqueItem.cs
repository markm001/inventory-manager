namespace InventoryManager.Core.Models;

public sealed record UniqueItem(string InstanceId, InventoryItem Item) : IInventoryEntry
{
    public string Id => InstanceId;
}