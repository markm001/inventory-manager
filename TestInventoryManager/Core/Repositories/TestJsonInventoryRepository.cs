using InventoryManager.Core.Models.DTO;
using Utils;

namespace TestInventoryManager.Core.Repositories;

[TestClass]
public class TestJsonInventoryRepository
{
    private string _filePath = null!;

    [TestInitialize]
    public void Setup()
    {
        _filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
    
    [TestMethod]
    public async Task LoadAsync_LoadDataFromJsonFile_ReturnsInventoryData()
    {
        var repository = new JsonRepository<InventoryData>("TestData/SampleInventory.json");

        InventoryData result = await repository.LoadAsync(TestContext.CancellationToken);

        Assert.HasCount(3, result.StackableItems);
        Assert.AreEqual(125000, result.StackableItems["GOLD"]);

        Assert.HasCount(3, result.UniqueItems);
        Assert.AreEqual("8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21", result.UniqueItems.First().InstanceId);
        Assert.AreEqual("IRON_ARMOR", result.UniqueItems.First().ItemId);
    }

    [TestMethod]
    public async Task LoadAsync_LoadStateDataFromJsonFile_ReturnsEquipmentStateData()
    {
        var repository = new JsonRepository<IReadOnlyList<ItemStateData>>("TestData/SampleEquipmentStates.json");

        IReadOnlyList<ItemStateData> stateData = await repository.LoadAsync(TestContext.CancellationToken);

        Assert.AreEqual("8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21", stateData[0].InstanceId);
        Assert.AreEqual(15, stateData[0].Level);
        Assert.HasCount(2, stateData[0].Slots);
    }

    [TestMethod]
    public async Task SaveAsync_CreatesEquipmentStatesFile()
    {
        var expected = "[{\"InstanceId\":\"UUID1\",\"Level\":15,\"Experience\":150,\"Slots\":[\"SLOT1\",\"SLOT2\"]}]";
        var repository = new JsonRepository<IReadOnlyList<ItemStateData>>(_filePath);

        var itemState = new ItemStateData("UUID1", 15, 150, ["SLOT1", "SLOT2"]);
        IReadOnlyList<ItemStateData> data = [itemState];
        await repository.SaveAsync(data, TestContext.CancellationToken);
        
        Assert.IsTrue(File.Exists(_filePath));
        Assert.AreEqual(expected, await File.ReadAllTextAsync(_filePath, TestContext.CancellationToken));
    }
    
    [TestMethod]
    public async Task SaveAsync_CreatesInventoryFile()
    {
        var expectedJson =
            "{\"StackableItems\":{\"GOLD\":125000,\"POTION\":25},\"UniqueItems\":[{\"InstanceId\":\"UUID1\",\"ItemId\":\"IRON_ARMOR\"}]}";
        var repository = new JsonRepository<InventoryData>(_filePath);

        var data = new InventoryData(new Dictionary<string, long>
            {
                ["GOLD"] = 125000,
                ["POTION"] = 25
            },
            [
                new UniqueInventoryData("UUID1", "IRON_ARMOR")
            ]);

        await repository.SaveAsync(data, TestContext.CancellationToken);

        Assert.IsTrue(File.Exists(_filePath));
        Assert.AreEqual(expectedJson, await File.ReadAllTextAsync(_filePath, TestContext.CancellationToken));
    }

    public TestContext TestContext { get; set; }
}