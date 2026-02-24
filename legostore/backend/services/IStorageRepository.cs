using LegoStore.Domain;

namespace LegoStore.Services;

/// <summary>
/// Persistence contract for saving and loading the warehouse <see cref="StoreStorage"/> state.
/// </summary>
public interface IStorageRepository
{
    /// <summary>
    /// Loads the storage state from the database.
    /// Returns <c>null</c> when no storage record exists yet.
    /// </summary>
    Task<StoreStorage?> LoadAsync(CancellationToken ct = default);

    /// <summary>
    /// Persists the current in-memory storage state to the database,
    /// replacing any previously stored record.
    /// </summary>
    Task SaveAsync(StoreStorage storage, CancellationToken ct = default);
}
