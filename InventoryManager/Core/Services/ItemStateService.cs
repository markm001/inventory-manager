using InventoryManager.Core.Models;

namespace InventoryManager.Core.Services;

public sealed class ItemStateService
{
    private readonly Dictionary<string, ItemState> _states;
    public IReadOnlyDictionary<string, ItemState> States => _states;

    public ItemStateService(IDictionary<string, ItemState> states)
    {
        ArgumentNullException.ThrowIfNull(states);

        _states = new Dictionary<string, ItemState>(states);
    }

    public bool Contains(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _states.ContainsKey(instanceId);
    }

    public ItemState? Get(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _states.GetValueOrDefault(instanceId);
    }

    public void Add(string instanceId, ItemState state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);
        ArgumentNullException.ThrowIfNull(state);

        if (!_states.TryAdd(instanceId, state))
        {
            throw new InvalidOperationException(
                $"State already exists for item instance '{instanceId}'.");
        }
    }

    public bool Remove(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _states.Remove(instanceId);
    }

    public void Update(string instanceId, ItemState state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);
        ArgumentNullException.ThrowIfNull(state);

        if (!_states.ContainsKey(instanceId))
        {
            throw new KeyNotFoundException(
                $"No state exists for item instance '{instanceId}'.");
        }

        _states[instanceId] = state;
    }
}