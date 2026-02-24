namespace LegoStore.Services;

/// <summary>
/// Parses BrickStore .bsx (XML) files to extract picked lot data.
/// </summary>
public interface IBsxParserService
{
    /// <summary>
    /// Parses a .bsx file at the given path and returns the list of picked lots.
    /// </summary>
    IEnumerable<PickedLot> Parse(string filePath);
}
