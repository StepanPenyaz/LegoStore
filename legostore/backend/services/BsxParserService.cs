using System.Xml.Linq;

namespace LegoStore.Services;

/// <inheritdoc cref="IBsxParserService"/>
public class BsxParserService : IBsxParserService
{
    /// <inheritdoc/>
    public IEnumerable<PickedLot> Parse(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"BSX file not found: {filePath}", filePath);

        var doc = XDocument.Load(filePath);

        return doc.Descendants("Item")
            .Select(item => new
            {
                LotId   = (string?)item.Element("LotID"),
                Quantity = (int?)item.Element("Qty")
            })
            .Where(x => x.LotId is not null && x.Quantity.HasValue)
            .Select(x => new PickedLot(x.LotId!, x.Quantity!.Value))
            .ToList();
    }
}
