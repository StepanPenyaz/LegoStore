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

    /// <summary>
    /// Subtracts <paramref name="quantity"/> from the section that holds <paramref name="lotId"/>
    /// inside the container identified by <paramref name="containerNumber"/>.
    /// Clears the section when its quantity reaches zero.
    /// </summary>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the container or the lot inside it cannot be found.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <paramref name="quantity"/> would make the section quantity negative.
    /// </exception>
    Task SubtractLotFromContainerAsync(int containerNumber, string lotId, int quantity, CancellationToken ct = default);
}
