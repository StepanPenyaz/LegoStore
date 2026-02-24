using LegoStore.Domain;

namespace LegoStore.Services;

/// <inheritdoc cref="IStorageService"/>
public class StorageService : IStorageService
{
    private readonly StoreStorage _storage;

    public StorageService(StoreStorage storage)
    {
        _storage = storage;
    }

    /// <inheritdoc/>
    public void ApplyPickedLots(IEnumerable<PickedLot> lots)
    {
        foreach (var lot in lots)
        {
            int remaining = lot.Quantity;
            foreach (var section in _storage.FindSectionsByLotId(lot.LotId))
            {
                if (remaining <= 0)
                    break;

                int toDeduct = Math.Min(remaining, section.Quantity);
                section.Deduct(toDeduct);
                remaining -= toDeduct;
            }
        }
    }

    /// <inheritdoc/>
    public IEnumerable<Section> GetEmptySections() =>
        _storage.Cabinets
            .SelectMany(cab => cab.Groups)
            .SelectMany(grp => grp.Cases)
            .SelectMany(c => c.Containers)
            .SelectMany(con => con.Sections)
            .Where(s => s.IsEmpty);

    /// <inheritdoc/>
    public IEnumerable<Container> GetEmptyContainers() =>
        _storage.Cabinets
            .SelectMany(cab => cab.Groups)
            .SelectMany(grp => grp.Cases)
            .SelectMany(c => c.Containers)
            .Where(c => c.IsEmpty);
}
