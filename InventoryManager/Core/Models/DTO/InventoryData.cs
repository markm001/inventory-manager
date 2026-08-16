namespace InventoryManager.Core.Models.DTO;

public record InventoryData(
    Dictionary<string, long> StackableItems,
    IEnumerable<UniqueInventoryData> UniqueItems
);

public sealed record UniqueInventoryData(
    string InstanceId,
    string ItemId
);