namespace InventoryManager.Core.Repositories;

public interface IRepository<TData>
{
    Task<TData> LoadAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(TData data, CancellationToken cancellationToken = default);
}