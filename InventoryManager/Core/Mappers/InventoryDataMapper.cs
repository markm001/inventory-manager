using InventoryManager.Core.Models;
using InventoryManager.Core.Models.DTO;

namespace InventoryManager.Core.Mappers;

public static class InventoryDataMapper
{
    public static InventoryData ToData(
        Inventory<ItemStack> stackableInventory,
        Inventory<UniqueItem> uniqueInventory)
    {
        ArgumentNullException.ThrowIfNull(stackableInventory);
        ArgumentNullException.ThrowIfNull(uniqueInventory);

        return new InventoryData(
            StackableItems: ToStackableData(stackableInventory),
            UniqueItems: ToUniqueData(uniqueInventory)
        );
    }

    private static Dictionary<string, long> ToStackableData(Inventory<ItemStack> inventory)
    {
        return inventory.Items
            .ToDictionary(item => item.Item.Id, item => item.Amount);
    }

    private static List<UniqueInventoryData> ToUniqueData(Inventory<UniqueItem> inventory)
    {
        return inventory.Items
            .Select(item => new UniqueInventoryData(item.InstanceId, item.Item.Id))
            .ToList();
    }
}