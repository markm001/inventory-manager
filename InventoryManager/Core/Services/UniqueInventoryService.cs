using InventoryManager.Core.Models;

namespace InventoryManager.Core.Services;

public class UniqueInventoryService
{
    private readonly Inventory<UniqueItem> _inventory;

    public UniqueInventoryService(Inventory<UniqueItem> inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        
        _inventory = inventory;
    }

    public bool Has(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _inventory.Items.Any(i => i.InstanceId == instanceId);
    }
    
    public UniqueItem? Get(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _inventory.Items.FirstOrDefault(item => item.InstanceId == instanceId);
    }
    
    public void Add(UniqueItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _inventory.Add(item);
    }

    public bool Remove(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        foreach (var item in _inventory.Items)
        {
            if (item.InstanceId != instanceId)
                continue;

            _inventory.Remove(item);
            return true;
        }
        
        return false;
    }
}