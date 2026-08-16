using InventoryManager.Core.Models;

namespace InventoryManager.Core.Validators;

public sealed class InventoryStateValidator
{
    public void Validate(Inventory<UniqueItem> inventory, IReadOnlyDictionary<string, ItemState> states)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(states);

        var instanceIds = inventory.Items
            .Select(item => item.InstanceId)
            .ToHashSet();

        foreach (string instanceId in states.Keys)
        {
            if (!instanceIds.Contains(instanceId))
            {
                throw new InvalidDataException(
                    $"Item state references unknown item instance '{instanceId}'.");
            }
        }
    }
}