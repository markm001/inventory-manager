using InventoryManager.Core.Models;
using InventoryManager.Core.Services;
using InventoryManager.Core.Validators;

namespace TestInventoryManager.Core.Validators;

[TestClass]
public class TestInventoryStateValidator
{
    [TestMethod]
    public void Validate_ValidUuidForBothItemAndState()
    {
        var uuid = Guid.NewGuid().ToString();
        var item = new UniqueItem(uuid, new InventoryItem("WEAPON", false));
        var inventory = new Inventory<UniqueItem>([item]);

        var states = new Dictionary<string, ItemState>()
        {
            { uuid, new ItemState(5, 50, ["QTM"]) }
        };

        var stateService = new StateService<ItemState>(states);

        var validator = new InventoryStateValidator<ItemState>();
        validator.Validate(inventory, stateService.States);
    }
    
    [TestMethod]
    public void Validate_InvalidUuid_ThrowsException()
    {
        var item = new UniqueItem("UUID", new InventoryItem("WEAPON", false));
        var itemTwo = new UniqueItem("UUID_2", new InventoryItem("ARMOR", false));
        var inventory = new Inventory<UniqueItem>([item,itemTwo]);

        var states = new Dictionary<string, ItemState>()
        {
            { "DIFF_UUID", new ItemState(5, 50, ["QTM"]) },
            { "UUID_2", new ItemState(5, 50, ["DHM"]) }
        };

        var stateService = new StateService<ItemState>(states);

        var validator = new InventoryStateValidator<ItemState>();
        Assert.Throws<InvalidDataException>(
            () => validator.Validate(inventory, stateService.States)
        );
    }
    
    [TestMethod]
    public void Validate_InventoryIsNull_ThrowsException()
    {
        var states = new Dictionary<string, ItemState>()
        {
            { "DIFF_UUID", new ItemState(5, 50, ["QTM"]) },
            { "UUID_2", new ItemState(5, 50, ["DHM"]) }
        };

        var stateService = new StateService<ItemState>(states);

        var validator = new InventoryStateValidator<ItemState>();
        Assert.Throws<ArgumentNullException>(
            () => validator.Validate(null, stateService.States)
        );
    }
    
    [TestMethod]
    public void Validate_StatesIsNull_ThrowsException()
    {
        var item = new UniqueItem("UUID", new InventoryItem("WEAPON", false));
        var itemTwo = new UniqueItem("UUID_2", new InventoryItem("ARMOR", false));
        var inventory = new Inventory<UniqueItem>([item,itemTwo]);

        var validator = new InventoryStateValidator<ItemState>();
        Assert.Throws<ArgumentNullException>(
            () => validator.Validate(inventory, null)
        );
    }
}