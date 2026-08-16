namespace InventoryManager.Core.Models;

public readonly record struct InventoryItem
{
    public string Id { get; }
    public bool IsStackable { get; }

    public InventoryItem(string id, bool isStackable)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Item ID cannot be null or empty.", nameof(id));

        Id = id;
        IsStackable = isStackable;
    }

    public override string ToString() => Id;
}