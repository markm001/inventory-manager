using InventoryManager.Core.Models;
using InventoryManager.Core.Models.DTO;

namespace InventoryManager.Core.Mappers;

public static class InventoryMapper
{
    public static Inventory<ItemStack> ToStackableInventory(InventoryData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return new Inventory<ItemStack>(data.StackableItems.Select(ToItemStack));
    }

    public static Inventory<UniqueItem> ToUniqueInventory(InventoryData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        return new Inventory<UniqueItem>(data.UniqueItems.Select(ToUniqueItem));
    }
    
    private static ItemStack ToItemStack(KeyValuePair<string, long> data)
    {
        return new ItemStack(
            new InventoryItem(data.Key, isStackable: true), 
            data.Value
        );
    }

    private static UniqueItem ToUniqueItem(UniqueInventoryData data)
    {
        return new UniqueItem(
            data.InstanceId,
            new InventoryItem(data.ItemId, isStackable: false)
        );
    }
}