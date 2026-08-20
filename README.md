# Inventory Manager

[![Build](https://img.shields.io/github/actions/workflow/status/markm001/inventory-manager/publish.yml)](https://github.com/markm001/inventory-manager/actions)
![GitHub Tag](https://img.shields.io/github/v/tag/markm001/inventory-manager)

A reusable inventory library designed to provide a decoupled foundation for game inventory systems.

The library handles inventory persistence and inventory operations for **stackable items**, **unique items**, and **per-instance item state**,
while leaving game-specific concepts such UI to higher-level systems.

The goal is to provide a reusable lower-level library that can be integrated into different games 
without rewriting the underlying inventory logic or relying on a specific Game Engine.

---

## Goals

The primary goals of this project are:

- Provide a reusable inventory system library for any C# compatible game.
- Keep the inventory system independent of the game engine and frontend.
- Support both stackable and unique items.
- Keep stackable and unique inventory semantics separate.
- Track unique item instances independently using an `InstanceId`.
- Store state belonging to unique item instances separately from the main inventory save data.
- Provide generic persistence infrastructure that can be reused for different save-data types.
- Make the system easy to test and extend.

---

## Architecture

The project follows a layered architecture:

```text
                         JSON / Save File
                                │
                                ▼
                      JsonRepository<TData>
                                │
                                ▼
                               DTOs
                                │
                                ▼
                           DataMapper
                                │
                                ▼
                          Domain Models
                                │
                   ┌────────────┴────────────┐
                   ▼                         ▼
               Inventory<T>              ItemState
                   │                         │
             ┌─────┴─────┐                   │
             ▼           ▼                   ▼
          Stackable    Unique          ItemStateService
          Service      Service
```

---

## Inventory

The generic `Inventory<T>` is the core collection used by the inventory system.

```csharp
public sealed class Inventory<T>
{
    public IReadOnlyList<T> Items { get; }
}
```

The collection is intentionally exposed as `IReadOnlyList<T>`. Consumers cannot directly modify the inventory.

---

## Stackable Items

Stackable items represent items where multiple units share the same inventory entry.

Examples:

```text
GOLD
SMALL_POTION
BLUE_CRYSTAL
```

The domain model is:

```csharp
public sealed record ItemStack(InventoryItem Item, long Amount);
```

A stackable Item-ID may only appear once in the inventory.

The `StackableInventoryService` handles operations such as:

```csharp
Add("GOLD", 500);
Remove("GOLD", 100);
GetAmount("GOLD");
Has("GOLD", 100);
```

---

## Unique Items

Unique items represent individual item instances.

For example:

```text
PHANTOM_SWORD
ADAMANTITE_SHIELD
```

may all have the same item definition while representing different physical instances.

```csharp
public sealed record UniqueItem(string InstanceId, InventoryItem Item);
```

Example:

```json
{
  "instanceId": "8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21",
  "itemId": "ADAMANTITE_SHIELD"
}
```

Two items may have the same `ItemId` but must have different `InstanceId` values, identifing the individual item.

The `UniqueInventoryService` provides operations such as:

```csharp
Add(item);
Remove(instanceId);
Get(instanceId);
Contains(instanceId);
```

---

## Item State

Unique items can have state that belongs to the individual instance.

```csharp
public sealed record ItemState(int Level, int Experience, IReadOnlyList<string> Slots);
```

For example:

```json
[
  {
    "instanceId": "8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21",
    "level": 15,
    "experience": 150,
    "slots": [
      "FIRE_MATERIA",
      "CRITICAL_MATERIA"
    ]
  }
]
```

State is deliberately kept separate from the inventory save data.
The inventory library does not interpret the meaning of the state.
That interpretation belongs to higher-level game systems.

---

## Instance Ids

Unique items use an `InstanceId` to distinguish individual instances.

For example:

```text
ItemId: ADAMANTITE_SHIELD

Instance A:
8d8c4f7a-2c8e-4a8a-8a44-8c0b5c9a5f21

Instance B:
4b6a1b30-9a4d-44b8-90d0-0f9d2a4a6e11
```

The `InstanceId` is the stable identity of the individual item. 
It can therefore be used by other systems to associate state with that exact item:

---

## Persistence

The library uses DTOs for save-file representation.

For example:

```csharp
public sealed record InventoryData(
    Dictionary<string, long> StackableItems,
    IReadOnlyList<UniqueInventoryData> UniqueItems
);
```

and:

```csharp
public sealed record UniqueInventoryData(
    string InstanceId,
    string ItemId
);
```

Examples for all data can be found [within the TestData](https://github.com/markm001/inventory-manager/tree/main/TestInventoryManager/TestData)

---

Persistence is implemented using a generic repository:

```csharp
public interface IRepository<TData>
{
    Task<TData> LoadAsync(
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        TData data,
        CancellationToken cancellationToken = default);
}
```

---

## Data Mapping

Mappers handle conversions.

`InventoryMapper`:
```text
InventoryData to Inventory<ItemStack> and Inventory<UniqueItem>
```

`InventoryDataMapper`:
```text
Inventory<ItemStack> or Inventory<UniqueItem> to InventoryData
```

---

## Validatiors

The `InventoryStateValidator` can be used to validate matching UUIDs between UniqueItems and ItemStates.
```csharp
var validator = new InventoryStateValidator();
validator.Validate(inventory, stateService.States);
```

---

## Example Usage

### Loading the Inventory and creating Services for `ItemStack` and `UniqueItem`:

```csharp
var repository = new JsonRepository<InventoryData>("TestData/SampleInventory.json");
InventoryData inventoryData = await repository.LoadAsync(TestContext.CancellationToken);

Inventory<ItemStack> stackInventory = InventoryMapper.ToStackableInventory(inventoryData);
Inventory<UniqueItem> uniqueInventory = InventoryMapper.ToUniqueInventory(inventoryData);

StackableInventoryService stackableService = new StackableInventoryService(stackInventory);
UniqueInventoryService uniqueService = new UniqueInventoryService(uniqueInventory);

//Data Operations here... e.g.
stackableService.Add("GOLD", 99);
uniqueService.Add(item);
```

### Saving the modified Inventory:

```csharp
InventoryData saveData = InventoryDataMapper.ToData(stackInventory, uniqueInventory);
await repository.SaveAsync(saveData, TestContext.CancellationToken);
```

---

### Loading the `ItemState` and creating Services:

```csharp
var repository = new JsonRepository<IReadOnlyList<ItemStateData>>("TestData/SampleEquipmentStates.json");
IReadOnlyList<ItemStateData> itemStateData = await repository.LoadAsync(TestContext.CancellationToken);

Dictionary<string, ItemState> itemStates = ItemStateMapper.ToItemState(itemStateData);

ItemStateService stateService = new ItemStateService(itemStates);

//TODO: State Operations here...
stateService.Add(uuid, new ItemState(100, ["QTM_X"]));
```

### Saving the modified State:
The states within the Service should be accessed via the read-only view *(service.States)*

```csharp
IReadOnlyList<ItemStateData> stateSaveData = ItemStateMapper.ToStateData(stateService.States);
await repository.SaveAsync(stateSaveData, TestContext.CancellationToken);
```

---

## Project Structure

```text
Core
│
├── Mappers
│   ├── InventoryDataMapper
│   ├── InventoryMapper
│   └── ItemStateMapper
│
├── Models
│   ├── Inventory
│   ├── InventoryItem
│   ├── ItemStack
│   ├── UniqueItem
│   ├── InventoryItem
│   ├── ItemState
│   └── DTO
│       ├── InventoryData
│       └── ItemStateData
│
└── Services
    ├── StackableInventoryService
    ├── UniqueInventoryService
    └── ItemStateService
```