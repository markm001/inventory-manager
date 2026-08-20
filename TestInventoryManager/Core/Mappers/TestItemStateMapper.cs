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
        int expectedExperience = 100;
        ItemStateData expected = new ItemStateData("UUID", expectedLevel, expectedExperience, expectedSlots);
        
        Dictionary<string, ItemState> itemStates = ItemStateMapper.ToItemState([expected]);
        
        Assert.IsNotNull(itemStates["UUID"]);
        Assert.AreEqual(expectedLevel,itemStates["UUID"].Level);
        Assert.AreEqual(expectedExperience,itemStates["UUID"].Experience);
        
        CollectionAssert.AreEqual(expectedSlots, itemStates["UUID"].Slots.ToList());
    }

    [TestMethod]
    public void ToStateData_ConvertStatesDictionary_ReturnsItemStateData()
    {
        string expectedUuid = "UUID";
        int expectedLevel = 10;
        int expectedExperience = 100;
        List<string> expectedSlots = ["SLOT_1","SLOT_2"];

        Dictionary<string, ItemState> itemStates =  new Dictionary<string, ItemState> {
            { expectedUuid, new ItemState(expectedLevel, expectedExperience, expectedSlots) }
        };

        IReadOnlyList<ItemStateData> actual = ItemStateMapper.ToStateData(itemStates);
        
        Assert.IsNotNull(actual);
        Assert.AreEqual(expectedUuid,actual[0].InstanceId);
        Assert.AreEqual(expectedLevel,actual[0].Level);
        Assert.AreEqual(expectedExperience,actual[0].Experience);
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