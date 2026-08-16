using InventoryManager.Core.Mappers;
using InventoryManager.Core.Models;

namespace TestInventoryManager.Core.Mappers;

[TestClass]
public class TestInventoryDataMapper
{
    [TestMethod]
    public void ToData_ConvertInventory_ReturnsInventoryData()
    {
        var expectedStack = new ItemStack(new InventoryItem("STACK",true), 24);
        Inventory<ItemStack> stack = new Inventory<ItemStack>([
            expectedStack
        ]);

        var expectedUnique = new UniqueItem("UUID", new InventoryItem("UNQ", false));
        Inventory<UniqueItem> unique = new Inventory<UniqueItem>([
            expectedUnique
        ]);

        var data = InventoryDataMapper.ToData(stack, unique);

        Assert.AreEqual(24, data.StackableItems["STACK"]);
        Assert.HasCount(1, data.UniqueItems);

        Assert.AreEqual("UUID",data.UniqueItems.First().InstanceId);
        Assert.AreEqual("UNQ",data.UniqueItems.First().ItemId);
    }

    [TestMethod]
    public void ToData_StackIsNull_ThrowsArgumentNullException()
    {
        Inventory<UniqueItem> unique = new Inventory<UniqueItem>([]);
        
        Assert.Throws<ArgumentNullException>(
            () => InventoryDataMapper.ToData(null, unique)
        );
    }
    
    [TestMethod]
    public void ToData_UniqueIsNull_ThrowsArgumentNullException()
    {
        Inventory<ItemStack> stack = new Inventory<ItemStack>([]);
        
        Assert.Throws<ArgumentNullException>(
            () => InventoryDataMapper.ToData(stack, null)
        );
    }
}