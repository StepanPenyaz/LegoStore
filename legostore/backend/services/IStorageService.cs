using LegoStore.Domain;

namespace LegoStore.Services;

/// <summary>
/// Represents a lot extracted from a BrickStore .bsx file.
/// </summary>
public record PickedLot(string LotId, int Quantity);

/// <summary>
/// Core storage service interface for applying picking results to the warehouse model.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Applies a collection of picked lots to the storage, deducting quantities from matching sections.
    /// </summary>
    void ApplyPickedLots(IEnumerable<PickedLot> lots);

    /// <summary>
    /// Returns all sections that are currently empty.
    /// </summary>
    IEnumerable<Section> GetEmptySections();

    /// <summary>
    /// Returns all containers where every section is empty.
    /// </summary>
    IEnumerable<Container> GetEmptyContainers();
}
