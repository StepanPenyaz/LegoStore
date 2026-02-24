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

        XDocument doc;
        try
        {
            doc = XDocument.Load(filePath);
        }
        catch (Exception ex) when (ex is System.Xml.XmlException or InvalidOperationException)
        {
            throw new InvalidOperationException($"Failed to parse BSX file '{filePath}': {ex.Message}", ex);
        }

        return doc.Descendants("Item")
            .Select(item => new
            {
                LotId    = (string?)item.Element("LotID"),
                Quantity = (int?)item.Element("Qty"),
                Remarks  = (string?)item.Element("Remarks")
            })
            .Where(x => x.LotId is not null && x.Quantity.HasValue)
            .Select(x => new PickedLot(
                x.LotId!,
                x.Quantity!.Value,
                ContainerInfoParser.TryParse(x.Remarks)))
            .ToList();
    }
}
