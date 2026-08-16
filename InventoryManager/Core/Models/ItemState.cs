namespace InventoryManager.Core.Models;

public sealed record ItemState
{
    public int Level { get; }
    public IReadOnlyList<string> Slots { get; }

    public ItemState(int level, IReadOnlyList<string> slots)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(level);
        ArgumentNullException.ThrowIfNull(slots);

        Slots = slots.ToList();
        Level = level;
    }
}