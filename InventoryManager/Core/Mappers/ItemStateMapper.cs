using InventoryManager.Core.Models;
using InventoryManager.Core.Models.DTO;

namespace InventoryManager.Core.Mappers;

public static class ItemStateMapper
{
    public static Dictionary<string, ItemState> ToItemState(IReadOnlyList<ItemStateData> equipStateData)
    {
        return equipStateData.ToDictionary(
            i => i.InstanceId, 
            i => new ItemState(i.Level, i.Experience,i.Slots)
        );
    }
    
    public static IReadOnlyList<ItemStateData> ToStateData(IReadOnlyDictionary<string, ItemState> states)
    {
        ArgumentNullException.ThrowIfNull(states);

        return states
            .Select(pair => new ItemStateData(pair.Key, pair.Value.Level, pair.Value.Experience, pair.Value.Slots))
            .ToList();
    }
}