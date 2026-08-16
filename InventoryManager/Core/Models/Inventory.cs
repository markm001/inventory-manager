namespace InventoryManager.Core.Models;

public sealed class Inventory<T> where T : IInventoryEntry
{
    private readonly List<T> _items;

    public IReadOnlyList<T> Items => _items;

    public Inventory(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = items.ToList();

        ValidateUniqueItems(_items);
    }

    internal void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Any(existing => existing.Id == item.Id))
        {
            throw new InvalidOperationException(
                $"Inventory already contains item '{item.Id}'.");
        }

        _items.Add(item);
    }

    internal bool Remove(T item)
    {
        return _items.Remove(item);
    }

    internal void Replace(int index, T item)
    {
        if (index < 0 || index >= _items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _items[index] = item;
    }

    private static void ValidateUniqueItems(
        IReadOnlyList<T> items)
    {
        var ids = new HashSet<string>();

        foreach (T item in items)
        {
            if (!ids.Add(item.Id))
            {
                throw new ArgumentException(
                    $"Duplicate inventory item ID: '{item.Id}'.",
                    nameof(items));
            }
        }
    }
}