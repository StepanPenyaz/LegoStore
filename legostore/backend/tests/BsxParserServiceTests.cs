using LegoStore.Services;

namespace LegoStore.Tests;

public class BsxParserServiceTests : IDisposable
{
    private readonly string _tempFile = Path.GetTempFileName();

    public void Dispose() => File.Delete(_tempFile);

    private void WriteXml(string content) => File.WriteAllText(_tempFile, content);

    private const string ValidBsx = """
        <?xml version="1.0" encoding="UTF-8"?>
        <BrickStoreXML>
          <Inventory>
            <Item>
              <LotID>507465701</LotID>
              <Qty>3</Qty>
              <Remarks>#1012#2</Remarks>
            </Item>
            <Item>
              <LotID>123456789</LotID>
              <Qty>1</Qty>
              <Remarks>#2200</Remarks>
            </Item>
            <Item>
              <LotID>999999999</LotID>
              <Qty>5</Qty>
            </Item>
          </Inventory>
        </BrickStoreXML>
        """;

    [Fact]
    public void Parse_ReturnsAllItemsWithValidLotAndQty()
    {
        WriteXml(ValidBsx);
        var service = new BsxParserService();

        var lots = service.Parse(_tempFile).ToList();

        Assert.Equal(3, lots.Count);
    }

    [Fact]
    public void Parse_PopulatesLotIdAndQuantity()
    {
        WriteXml(ValidBsx);
        var service = new BsxParserService();

        var lots = service.Parse(_tempFile).ToList();

        Assert.Equal("507465701", lots[0].LotId);
        Assert.Equal(3, lots[0].Quantity);
    }

    [Fact]
    public void Parse_ExtractsContainerInfo_WhenRemarksPresent()
    {
        WriteXml(ValidBsx);
        var service = new BsxParserService();

        var lots = service.Parse(_tempFile).ToList();

        Assert.NotNull(lots[0].ContainerInfo);
        Assert.Equal(1012, lots[0].ContainerInfo!.ContainerNumber);
        Assert.Equal(2, lots[0].ContainerInfo!.OccupiedSections);
    }

    [Fact]
    public void Parse_ContainerInfo_DefaultsToOneSections_ForSingleHashRemarks()
    {
        WriteXml(ValidBsx);
        var service = new BsxParserService();

        var lots = service.Parse(_tempFile).ToList();

        Assert.NotNull(lots[1].ContainerInfo);
        Assert.Equal(2200, lots[1].ContainerInfo!.ContainerNumber);
        Assert.Equal(1, lots[1].ContainerInfo!.OccupiedSections);
    }

    [Fact]
    public void Parse_ContainerInfo_IsNull_WhenNoRemarksElement()
    {
        WriteXml(ValidBsx);
        var service = new BsxParserService();

        var lots = service.Parse(_tempFile).ToList();

        Assert.Null(lots[2].ContainerInfo);
    }

    [Fact]
    public void Parse_ThrowsFileNotFoundException_WhenFileMissing()
    {
        var service = new BsxParserService();

        Assert.Throws<FileNotFoundException>(() =>
            service.Parse("/nonexistent/path/Order_999.bsx"));
    }

    [Fact]
    public void Parse_ThrowsInvalidOperationException_WhenXmlMalformed()
    {
        WriteXml("<<not valid xml>>");
        var service = new BsxParserService();

        Assert.Throws<InvalidOperationException>(() => service.Parse(_tempFile));
    }
}
