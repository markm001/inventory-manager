namespace InventoryManager.Core.Models.DTO;

public record ItemStateData(
        string InstanceId,
        int Level,
        int Experience,
        IReadOnlyList<string> Slots
);