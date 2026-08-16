using InventoryManager.Core.Mappers;
using InventoryManager.Core.Models;
using InventoryManager.Core.Models.DTO;

namespace TestInventoryManager.Core.Mappers;

[TestClass]
public class TestInventoryMapper
{
    [TestMethod]
    public void ToStackableInventory_ConvertInventoryData_ReturnsItemStacks()
    {
        var dict = new Dictionary<string, long>()
        {
            {"TEST_1", 1},
            {"TEST_2", 10}
        };
        var inventoryData = new InventoryData(
            dict,
            []
        );
        
        Inventory<ItemStack> inventory = InventoryMapper.ToStackableInventory(inventoryData);

        foreach (var itemStack in inventory.Items)
        {
            Assert.AreEqual(itemStack.Amount, dict[itemStack.Id]);
        }
    }
    
    [TestMethod]
    public void ToStackableInventory_NullData_ThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
         () => InventoryMapper.ToStackableInventory(null)
        );
    }
    
    [TestMethod]
    public void ToUniqueInventory_ConvertInventoryData_ReturnsUniqueItems()
    {
        var expected = new UniqueInventoryData("UUID_1", "TEST_1");

        var inventoryData = new InventoryData(
            null,
            [
                expected,
                new UniqueInventoryData("UUID_2","TEST_2")
            ]
        );
        
        Inventory<UniqueItem> inventory = InventoryMapper.ToUniqueInventory(inventoryData);

        Assert.AreEqual(expected.InstanceId,inventory.Items[0].InstanceId);
        Assert.AreEqual(expected.InstanceId,inventory.Items[0].Id);
        Assert.AreEqual(expected.ItemId,inventory.Items[0].Item.Id);
        Assert.IsFalse(inventory.Items[0].Item.IsStackable);
    }
    
    [TestMethod]
    public void ToUniqueInventory_NullData_ThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => InventoryMapper.ToUniqueInventory(null)
        );
    }
}