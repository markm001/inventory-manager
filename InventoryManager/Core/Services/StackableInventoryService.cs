using InventoryManager.Core.Models;

namespace InventoryManager.Core.Services;

public sealed class StackableInventoryService
{
    private readonly Inventory<ItemStack> _inventory;

    public StackableInventoryService(Inventory<ItemStack> inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        _inventory = inventory;
    }

    public long GetAmount(string itemId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemId);

        ItemStack? stack = _inventory.Items
            .FirstOrDefault(x => x.Item.Id == itemId);

        return stack?.Amount ?? 0;
    }

    public bool Has(string itemId, long amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemId);
        ArgumentOutOfRangeException.ThrowIfNegative(amount);

        return GetAmount(itemId) >= amount;
    }

    public void Add(string itemId, long amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            ItemStack stack = _inventory.Items[i];

            if (stack.Item.Id != itemId)
                continue;

            _inventory.Replace(i, stack with
                {
                    Amount = stack.Amount + amount
                });

            return;
        }

        _inventory.Add(new ItemStack(
            new InventoryItem(itemId, true),
            amount)
        );
    }
    
    public bool Remove(string itemId, long amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            ItemStack stack = _inventory.Items[i];

            if (stack.Item.Id != itemId)
                continue;

            if (stack.Amount < amount)
                return false;

            long remaining = stack.Amount - amount;
            
            if (remaining == 0)
            {
                _inventory.Remove(stack);
            }
            else
            {
                _inventory.Replace(i, stack with
                    {
                        Amount = remaining
                    });
            }
            return true;
        }
        return false;
    }
}