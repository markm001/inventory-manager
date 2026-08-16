using InventoryManager.Core.Models;

namespace TestInventoryManager.Core.Models;

[TestClass]
public class TestInventory
{
    [TestMethod]
    public void Inventory_NullItems_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new Inventory<ItemStack>(null)
        );
        
        Assert.Throws<ArgumentNullException>(
            () => new Inventory<UniqueItem>(null)
        );
    }
    
    [TestMethod]
    public void Inventory_DuplicateStackInventoryItemId_ThrowsException()
    {
        Assert.Throws<ArgumentException>(
            () => new Inventory<ItemStack>([
                new ItemStack(new InventoryItem("GOLD", true), 400),
                new ItemStack(new InventoryItem("GOLD", true), 200),
            ])
        );
    }
    
    [TestMethod]
    public void Inventory_DuplicateUniqueInventoryItemUuid_ThrowsException()
    {
        Assert.Throws<ArgumentException>(
            () => new Inventory<UniqueItem>([
                new UniqueItem("UUID1", new InventoryItem("I1", false)),
                new UniqueItem("UUID1", new InventoryItem("I1", false))
            ])
        );
    }
}