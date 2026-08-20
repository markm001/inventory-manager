namespace InventoryManager.Core.Models;

public sealed record ItemState
{
    public int Level { get; }
    public int Experience { get; }
    public IReadOnlyList<string> Slots { get; }

    public ItemState(int level, int experience, IReadOnlyList<string> slots)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(level);
        ArgumentNullException.ThrowIfNull(slots);

        Slots = slots.ToList();
        Experience = experience;
        Level = level;
    }
}