using InventoryManager.Core.Mappers;
using InventoryManager.Core.Models;
using InventoryManager.Core.Models.DTO;
using InventoryManager.Core.Repositories;
using InventoryManager.Core.Services;

namespace TestInventoryManager.Core;

[TestClass]
public class TestInventoryManager
{
    public TestContext TestContext { get; set; }
    
    [TestMethod]
    public async Task Test_InventoryManager()
    {
        string inventoryPath = Path.GetTempFileName();
        string statePath = Path.GetTempFileName();
        
        string uuid = Guid.NewGuid().ToString();
        UniqueItem item = new UniqueItem(uuid, new InventoryItem("TEST_ITEM", false));

        // > Reading Inventory
        var inventoryRepository = new JsonRepository<InventoryData>("TestData/SampleInventory.json");
        InventoryData inventoryData = await inventoryRepository.LoadAsync(TestContext.CancellationToken);

        Inventory<ItemStack> stackInventory = InventoryMapper.ToStackableInventory(inventoryData);
        Inventory<UniqueItem> uniqueInventory = InventoryMapper.ToUniqueInventory(inventoryData);

        StackableInventoryService stackableService = new StackableInventoryService(stackInventory);
        UniqueInventoryService uniqueService = new UniqueInventoryService(uniqueInventory);
        
        //TODO: Data Operations here...
        stackableService.Add("GOLD", 99);
        uniqueService.Add(item);

        // > Reading Item-State
        var stateRepository = new JsonRepository<IReadOnlyList<ItemStateData>>("TestData/SampleEquipmentStates.json");
        IReadOnlyList<ItemStateData> itemStateData = await stateRepository.LoadAsync(TestContext.CancellationToken);

        Dictionary<string, ItemState> itemStates = ItemStateMapper.ToItemState(itemStateData);

        ItemStateService stateService = new ItemStateService(itemStates);
        
        //TODO: State Operations here...
        stateService.Add(uuid, new ItemState(100, ["QTM_X"]));

        try
        {
            // > Writing Inventory
            InventoryData inventorySaveData = InventoryDataMapper.ToData(stackInventory, uniqueInventory);
            JsonRepository<InventoryData> invRepository = new JsonRepository<InventoryData>(inventoryPath);
            await invRepository.SaveAsync(inventorySaveData, TestContext.CancellationToken);
            
            
            string invContent = await File.ReadAllTextAsync(inventoryPath, TestContext.CancellationToken);
            Console.WriteLine("--- INVENTORY ---");
            Console.WriteLine(invContent);
            
            // > Writing Item-State
            IReadOnlyList<ItemStateData> stateSaveData = ItemStateMapper.ToStateData(stateService.States);
            
            JsonRepository<IReadOnlyList<ItemStateData>> stateDataRepository = new JsonRepository<IReadOnlyList<ItemStateData>>(statePath);
            await stateDataRepository.SaveAsync(stateSaveData, TestContext.CancellationToken);
            
            string stateContent = await File.ReadAllTextAsync(statePath, TestContext.CancellationToken);
            Console.WriteLine("--- STATE ---");
            Console.WriteLine(stateContent);
        }
        finally
        {
            File.Delete(inventoryPath);
            File.Delete(statePath);
        }
    }

}