using InventoryManager.Core.Models;

namespace InventoryManager.Core.Services;

public sealed class StateService<T> where T : StateRecord
{
    private readonly Dictionary<string, T> _states;
    public IReadOnlyDictionary<string, T> States => _states;

    public StateService(IDictionary<string, T> states)
    {
        ArgumentNullException.ThrowIfNull(states);

        _states = new Dictionary<string, T>(states);
    }

    public bool Contains(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _states.ContainsKey(instanceId);
    }

    public T? Get(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _states.GetValueOrDefault(instanceId);
    }

    public void Add(string instanceId, T state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);
        ArgumentNullException.ThrowIfNull(state);

        if (!_states.TryAdd(instanceId, state))
        {
            throw new InvalidOperationException(
                $"State already exists for the instance '{instanceId}'.");
        }
    }

    public bool Remove(string instanceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);

        return _states.Remove(instanceId);
    }

    public void Update(string instanceId, T state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceId);
        ArgumentNullException.ThrowIfNull(state);

        if (!_states.ContainsKey(instanceId))
        {
            throw new KeyNotFoundException(
                $"No state exists for the instance '{instanceId}'.");
        }

        _states[instanceId] = state;
    }
}