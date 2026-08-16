using System.Text.Json;
using InventoryManager.Core.Mappers;
using InventoryManager.Core.Models;
using InventoryManager.Core.Models.DTO;
using InventoryData = InventoryManager.Core.Models.DTO.InventoryData;

namespace TestInventoryManager.Core.Repositories;

[TestClass]
public class TestSerialization
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    
    [TestMethod]
    public void DeserializeInventoryData()
    {
        string json = File.ReadAllText("TestData/SampleInventory.json");

        InventoryData inventoryData = JsonSerializer.Deserialize<InventoryData>(json, _options) 
                                      ?? throw new InvalidDataException("Invalid inventory data.");

        var unique = InventoryMapper.ToUniqueInventory(inventoryData);
        var stackable = InventoryMapper.ToStackableInventory(inventoryData);
        
        Assert.HasCount(3, stackable.Items);
        Assert.HasCount(3, unique.Items);
         
        Assert.AreEqual("GOLD", stackable.Items[0].Item.Id);
        Assert.AreEqual(125000, stackable.Items[0].Amount);
         
        Assert.AreEqual("IRON_ARMOR", unique.Items[0].Item.Id);
        Assert.AreEqual("8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21", unique.Items[0].InstanceId);
    }

    [TestMethod]
    public void SerializeInventoryData()
    {
        var expectedOutput = "{\"StackableItems\":{\"STACK1\":5,\"STACK2\":3},\"UniqueItems\":[{\"InstanceId\":\"UUID1\",\"ItemId\":\"UNQ1\"},{\"InstanceId\":\"UUID2\",\"ItemId\":\"UNQ2\"}]}";
        var stacks = new Inventory<ItemStack>([
            new ItemStack(new InventoryItem("STACK1", true), 5),
            new ItemStack(new InventoryItem("STACK2", true), 3),
        ]);
        var uniques = new Inventory<UniqueItem>([
            new UniqueItem("UUID1", new InventoryItem("UNQ1", false)),
            new UniqueItem("UUID2", new InventoryItem("UNQ2", false))
        ]);

        var inventoryData = InventoryDataMapper.ToData(stacks, uniques);
        var inventoryJson = JsonSerializer.Serialize(inventoryData, _options);
        
        Assert.AreEqual(expectedOutput, inventoryJson);
    }
    
    [TestMethod]
    public void DeserializeEquipmentStateData()
    {
        string json = File.ReadAllText("TestData/SampleEquipmentStates.json");

        IReadOnlyList<ItemStateData> equipStateData =
            JsonSerializer.Deserialize<IReadOnlyList<ItemStateData>>(json, _options)
            ?? throw new InvalidDataException("Invalid Equipment State data.");

        var itemStates = ItemStateMapper.ToItemState(equipStateData);

        ItemState itemState = itemStates["8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21"];
        
        Assert.IsNotNull(itemState);
        Assert.AreEqual(15, itemState.Level);
        Assert.HasCount(2, itemState.Slots);
        foreach (string slot in itemState.Slots)
        {
            Assert.Contains(slot, ["FIRE_MATERIA","CRITICAL_MATERIA"]);
        }
    }

    [TestMethod]
    public void SerializeItemStateData()
    {
        var expectedOutput ="[{\"InstanceId\":\"8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21\",\"Level\":15,\"Slots\":[\"FIRE_MATERIA\",\"CRITICAL_MATERIA\"]},{\"InstanceId\":\"ABC\",\"Level\":5,\"Slots\":[\"MATERIA_X\",\"MATERIA_Y\"]}]";
        var itemStates = new Dictionary<string,ItemState>{
            { "8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21", new ItemState(15, ["FIRE_MATERIA","CRITICAL_MATERIA"]) },
            { "ABC", new ItemState(5, ["MATERIA_X","MATERIA_Y"]) },
        };

        var itemStateData = ItemStateMapper.ToStateData(itemStates);
        var stateJson = JsonSerializer.Serialize(itemStateData, _options);
        
        Assert.AreEqual(expectedOutput, stateJson);
    }
}