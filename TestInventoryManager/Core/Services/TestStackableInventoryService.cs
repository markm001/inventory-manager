using InventoryManager.Core.Models;
using InventoryManager.Core.Services;

namespace TestInventoryManager.Core.Services;

[TestClass]
public class TestStackableInventoryService
{
    [TestMethod]
    public void TestStackableInventoryService_Operations()
    {
        Inventory<ItemStack> stackableInventory = new Inventory<ItemStack>([
            new ItemStack(new InventoryItem("GOLD", true), 400),
            new ItemStack(new InventoryItem("CRYSTAL", true), 10)
        ]);
        StackableInventoryService service = new StackableInventoryService(stackableInventory);
        
        service.Add("WATER", 2);
        Assert.AreEqual(2,service.GetAmount("WATER"));
        
        service.Add("GOLD", 500);
        Assert.IsTrue(service.Has("GOLD", 900));
        Assert.IsTrue(service.Remove("GOLD", 100));
        Assert.AreEqual(800,service.GetAmount("GOLD"));
        Assert.IsFalse(service.Remove("GOLD", 1000));
    }
    
    // GET AMOUNT
    [TestMethod]
    public void GetAmount_ItemIdWhiteSpace_ThrowsException()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.Throws<ArgumentException>(
            () => service.GetAmount("")
        );
    }
    
    [TestMethod]
    public void GetAmount_NonExistingItemId_ReturnZero()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        long amount = service.GetAmount("TEST");
        
        Assert.AreEqual(0,amount);
    }
    
    // HAS
    [TestMethod]
    public void Has_ItemIdWhiteSpace_ThrowsException()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.Throws<ArgumentException>(
            () => service.Has("",5)
        );
    }
    
    [TestMethod]
    public void Has_WithNegativeAmount_ThrowsException()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => service.Has("TEST",-5)
        );
    }
    
    [TestMethod]
    public void Has_InsufficientAmount_ReturnFalse()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([
            new ItemStack(new InventoryItem("GOLD", true), 400),
        ]);
        var service = new StackableInventoryService(stacks);

        Assert.IsFalse(service.Has("GOLD", 500));
    }
    
    // ADD
    [TestMethod]
    public void Add_ItemIdWhiteSpace_ThrowsException()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.Throws<ArgumentException>(
            () => service.Add("",5)
        );
    }
    
    [TestMethod]
    public void Add_AmountIsNegative_ThrowsException()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => service.Add("WATER",-5)
        );
    }
    
    [TestMethod]
    public void Add_ExistingItem_ReplaceWithCombinedAmount()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([
            new ItemStack(new InventoryItem("GOLD", true), 400)
        ]);
        var service = new StackableInventoryService(stacks);

        service.Add("GOLD", 600);
        
        Assert.AreEqual(1000, service.GetAmount("GOLD"));
    }
    
    // REMOVE
    [TestMethod]
    public void Remove_ItemIdWhiteSpace_ThrowsException()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.Throws<ArgumentException>(
            () => service.Remove("",1)
        );
    }
    
    [TestMethod]
    public void Remove_AmountIsNegative_ThrowsException()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => service.Remove("TEST",-1)
        );
    }
    
    [TestMethod]
    public void Remove_AmountToRemoveHigherThanExistingAmount_ReturnsFalse()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([
            new ItemStack(new InventoryItem("GOLD", true), 400)
        ]);
        var service = new StackableInventoryService(stacks);

        Assert.IsFalse(service.Remove("GOLD",500));
    }
    
    [TestMethod]
    public void Remove_RemoveExactAmountRemainingAmountIsZero_RemoveStack()
    {
        long toRemove = 500;
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([
            new ItemStack(new InventoryItem("GOLD", true), toRemove),
            new ItemStack(new InventoryItem("CRYSTAL", true), 100)
        ]);
        var service = new StackableInventoryService(stacks);

        Assert.IsTrue(service.Remove("GOLD", toRemove));
        Assert.AreEqual(0,service.GetAmount("GOLD"));
        
        foreach (var stacksItem in stacks.Items)
        {
            Assert.AreNotEqual("GOLD", stacksItem.Id);
        }
    }
    
    [TestMethod]
    public void Remove_NonExistingItemId_ReturnsFalse()
    {
        Inventory<ItemStack> stacks = new Inventory<ItemStack>([]);
        var service = new StackableInventoryService(stacks);

        Assert.IsFalse(service.Remove("ABC",500));
    }
}