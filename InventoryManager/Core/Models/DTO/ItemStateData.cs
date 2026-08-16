namespace InventoryManager.Core.Models.DTO;

public record ItemStateData(
        string InstanceId,
        int Level,
        IReadOnlyList<string> Slots
);