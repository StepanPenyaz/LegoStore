namespace LegoStore.Domain;

/// <summary>
/// Represents the full warehouse storage composed of N <see cref="Cabinet"/>s.
/// </summary>
public class StoreStorage
{
    public IReadOnlyList<Cabinet> Cabinets { get; }

    public StoreStorage(IEnumerable<Cabinet> cabinets)
    {
        Cabinets = cabinets.ToList().AsReadOnly();
    }

    /// <summary>
    /// Finds all sections across all cabinets that hold the given <paramref name="lotId"/>.
    /// </summary>
    public IEnumerable<Section> FindSectionsByLotId(string lotId) =>
        Cabinets
            .SelectMany(cab => cab.Groups)
            .SelectMany(grp => grp.Cases)
            .SelectMany(c => c.Containers)
            .SelectMany(con => con.Sections)
            .Where(s => s.LotId == lotId);
}
