using System.Text.Json;

namespace InventoryManager.Core.Repositories;

public sealed class JsonRepository<TData> : IRepository<TData>
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    public JsonRepository(string filePath, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        _filePath = filePath;
        _options = options ?? new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<TData> LoadAsync(CancellationToken cancellationToken = default)
    {
        await using FileStream stream = File.OpenRead(_filePath);

        return await JsonSerializer.DeserializeAsync<TData>(
                   stream,
                   _options,
                   cancellationToken)
               ?? throw new InvalidDataException($"Could not deserialize {typeof(TData).Name}.");
    }

    public async Task SaveAsync(TData data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using FileStream stream = File.Create(_filePath);

        await JsonSerializer.SerializeAsync(
            stream,
            data,
            _options,
            cancellationToken
        );
    }
}