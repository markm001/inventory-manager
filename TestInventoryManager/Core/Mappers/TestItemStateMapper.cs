using System.Runtime.InteropServices;
using InventoryManager.Core.Mappers;
using InventoryManager.Core.Models;
using InventoryManager.Core.Models.DTO;

namespace TestInventoryManager.Core.Mappers;

[TestClass]
public class TestItemStateMapper
{
    [TestMethod]
    public void ToItemState_ConvertItemStateData_ReturnsDictionary()
    {
        List<string> expectedSlots = ["SLOT_1","SLOT_2"];
        int expectedLevel = 10;
        ItemStateData expected = new ItemStateData("UUID", expectedLevel, expectedSlots);
        
        Dictionary<string, ItemState> itemStates = ItemStateMapper.ToItemState([expected]);
        
        Assert.IsNotNull(itemStates["UUID"]);
        Assert.AreEqual(expectedLevel,itemStates["UUID"].Level);
        
        CollectionAssert.AreEqual(expectedSlots, itemStates["UUID"].Slots.ToList());
    }

    [TestMethod]
    public void ToStateData_ConvertStatesDictionary_ReturnsItemStateData()
    {
        string expectedUuid = "UUID";
        int expectedLevel = 10;
        List<string> expectedSlots = ["SLOT_1","SLOT_2"];

        Dictionary<string, ItemState> itemStates =  new Dictionary<string, ItemState> {
            { expectedUuid, new ItemState(expectedLevel, expectedSlots) }
        };

        IReadOnlyList<ItemStateData> actual = ItemStateMapper.ToStateData(itemStates);
        
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedUuid,actual[0].InstanceId);
        Assert.AreEqual(expectedLevel,actual[0].Level);
        CollectionAssert.AreEqual(expectedSlots,actual[0].Slots.ToList());
    }

    [TestMethod]
    public void ToStateData_EmptyDictionary_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => ItemStateMapper.ToStateData(null)
        );
    }
}