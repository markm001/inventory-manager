using InventoryManager.Core.Models;

namespace InventoryManager.Core.Validators;

public sealed class InventoryStateValidator<T> where T : StateRecord
{
    public void Validate(Inventory<UniqueItem> inventory, IReadOnlyDictionary<string, T> states)
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
                    $"State references unknown instance '{instanceId}'.");
            }
        }
    }
}