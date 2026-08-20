using InventoryManager.Core.Models;
using InventoryManager.Core.Services;

namespace TestInventoryManager.Core.Services;

[TestClass]
public class TestItemStateService
{
    private readonly IDictionary<string, ItemState> _states = new Dictionary<string, ItemState>()
    {
        { "UUID1", new ItemState(10, 100, ["SLOT1"]) }
    };
    
    [TestMethod]
    public void TestItemStateService_Operations()
    {
        var expectedState = new ItemState(5, 50, ["SLOT2"]);
        var expectedUuid = "UUID2";

        Dictionary<string, ItemState> states = new Dictionary<string, ItemState>
        {
            { "UUID1", new ItemState(10, 100, ["SLOT1"]) }
        };

        ItemStateService service = new ItemStateService(states);

        service.Add(expectedUuid, expectedState);
        Assert.AreEqual(expectedState, service.Get(expectedUuid));
        
        Assert.IsTrue(service.Remove(expectedUuid));
        Assert.IsFalse(service.Contains(expectedUuid));
        
        service.Update("UUID1", expectedState);
        Assert.AreEqual(expectedState, service.Get("UUID1"));
    }
    
    [TestMethod]
    public void TestItemStateService_StateIsNull_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentNullException>(() => new ItemStateService(null));
    }
    
    // CONTAINS
    [TestMethod]
    public void Contains_InstanceIdWhiteSpace_ThrowsArgumentException()
    {
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<ArgumentException>(
            () => service.Contains("")
        );
    }
    
    // GET
    [TestMethod]
    public void Get_InstanceIdWhiteSpace_ThrowsArgumentException()
    {
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<ArgumentException>(
            () => service.Get("")
        );
    }
    
    [TestMethod]
    public void Get_NonExistingInstanceId_ReturnsNull()
    {
        ItemStateService service = new ItemStateService(_states);

        Assert.IsNull(service.Get("ABC"));
    }
    
    // ADD
    [TestMethod]
    public void Add_InstanceIdWhiteSpace_ThrowsArgumentException()
    {
        ItemState itemState = new ItemState(10, 100, ["SLOT1"]);
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<ArgumentException>(
            () => service.Add("", itemState)
        );
    }
    
    [TestMethod]
    public void Add_StateNull_ThrowsArgumentNullException()
    {
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<ArgumentNullException>(
            () => service.Add("TEST", null)
        );
    }
    
    [TestMethod]
    public void Add_UuidExists_ThrowsInvalidOperationException()
    {
        ItemState itemState = new ItemState(10, 100, ["SLOT1"]);
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<InvalidOperationException>(
            () => service.Add("UUID1", itemState)
        );
    }
    
    // REMOVE
    [TestMethod]
    public void Remove_InstanceIdWhiteSpace_ThrowsArgumentException()
    {
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<ArgumentException>(
            () => service.Remove("")
        );
    }
    
    // UPDATE
    [TestMethod]
    public void Update_InstanceIdWhiteSpace_ThrowsArgumentException()
    {
        ItemState itemState = new ItemState(10, 100, ["SLOT1"]);
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<ArgumentException>(
            () => service.Update("", itemState)
        );
    }
    
    [TestMethod]
    public void Update_StateIsNull_ThrowsArgumentNullException()
    {
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<ArgumentException>(
            () => service.Update("UUID1", null)
        );
    }
    
    [TestMethod]
    public void Update_NonExistingInstanceId_ThrowsKeyNotFoundException()
    {
        ItemState itemState = new ItemState(10, 100, ["SLOT1"]);
        ItemStateService service = new ItemStateService(_states);

        Assert.Throws<KeyNotFoundException>(
            () => service.Update("NOT_EXISTS", itemState)
        );
    }
}