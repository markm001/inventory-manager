using InventoryManager.Core.Models;
using InventoryManager.Core.Services;

namespace TestInventoryManager.Core.Services;

[TestClass]
public class TestUniqueInventoryService
{
    [TestMethod]
    public void TestUniqueInventoryService_Operations()
    {
        UniqueItem expected = new UniqueItem("UUID1", new InventoryItem("UNQ1", false));
        Inventory<UniqueItem> inventory = new Inventory<UniqueItem>([
            expected,
            new UniqueItem("UUID2", new InventoryItem("UNQ2", false)),
        ]);
        UniqueInventoryService service = new UniqueInventoryService(inventory);

        UniqueItem item = new UniqueItem("UUID3", new InventoryItem("UNQ3", false));
        
        service.Add(item);
        Assert.IsTrue(service.Has("UUID3"));

        Assert.IsTrue(service.Remove("UUID3"));
        Assert.IsFalse(service.Has("UUID3"));

        UniqueItem? actual = service.Get("UUID1");

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    // HAS
    [TestMethod]
    public void Has_ItemIdWhiteSpace_ThrowsException()
    {
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([]);
        var service = new UniqueInventoryService(uniques);

        Assert.Throws<ArgumentException>(
            () => service.Has("")
        );
    }
    
    [TestMethod]
    public void Has_NotExistingItem_ReturnsFalse()
    {
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([]);
        var service = new UniqueInventoryService(uniques);

        Assert.IsFalse(service.Has("TEST"));
    }
    
    // GET
    [TestMethod]
    public void Get_ItemIdWhiteSpace_ThrowsException()
    {
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([]);
        var service = new UniqueInventoryService(uniques);

        Assert.Throws<ArgumentException>(
            () => service.Get("")
        );
    }
    
    [TestMethod]
    public void Get_NotExistingItem_ReturnsNull()
    {
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([]);
        var service = new UniqueInventoryService(uniques);

        Assert.IsNull(service.Get("TEST"));
    }
    
    // ADD
    [TestMethod]
    public void Add_ItemIsNull_ThrowsException()
    {
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([]);
        var service = new UniqueInventoryService(uniques);

        Assert.Throws<ArgumentNullException>(
            () => service.Add(null)
        );
    }
    
    [TestMethod]
    public void Add_ExistingItem_ThrowsException()
    {
        var item = new UniqueItem("UUID1", new InventoryItem("TEST", false));
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([item]);
        var service = new UniqueInventoryService(uniques);

        Assert.Throws<InvalidOperationException>(
            () => service.Add(item)
        );
    }
    
    // REMOVE
    [TestMethod]
    public void Remove_InstanceIdWhiteSpace_ThrowsException()
    {
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([]);
        var service = new UniqueInventoryService(uniques);

        Assert.Throws<ArgumentException>(
            () => service.Remove("")
        );
    }
    
    [TestMethod]
    public void Remove_WithValidUuid_ReturnsTrue()
    {
        var item = new UniqueItem("TEST_UUID", new InventoryItem("TEST", false));
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([item]);
        var service = new UniqueInventoryService(uniques);

        Assert.IsTrue(service.Remove("TEST_UUID"));
        Assert.HasCount(0, uniques.Items);
    }
    
    [TestMethod]
    public void Remove_NoItemsInInventory_ReturnsFalse()
    {
        Inventory<UniqueItem> uniques = new Inventory<UniqueItem>([]);
        var service = new UniqueInventoryService(uniques);

        Assert.IsFalse(service.Remove("TEST_UUID"));
    }
}