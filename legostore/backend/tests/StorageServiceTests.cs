using LegoStore.Domain;
using LegoStore.Services;

namespace LegoStore.Tests;

public class StorageServiceTests
{
    private static CaseGroup MakeGroup() =>
        new CaseGroup(Enumerable.Range(0, 9).Select(_ => new Case(ContainerType.PX6)));

    private static StoreStorage BuildStorage()
    {
        var cabinet = new Cabinet(Enumerable.Range(0, 4).Select(_ => MakeGroup()));
        return new StoreStorage(new[] { cabinet });
    }

    [Fact]
    public void ApplyPickedLots_DeductsQuantityFromSection()
    {
        var storage = BuildStorage();
        var section = storage.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0];
        section.Assign("LOT-001", 10);

        var service = new StorageService(storage);
        service.ApplyPickedLots(new[] { new PickedLot("LOT-001", 4) });

        Assert.Equal(6, section.Quantity);
    }

    [Fact]
    public void ApplyPickedLots_SectionBecomesEmpty_WhenFullyDeducted()
    {
        var storage = BuildStorage();
        var section = storage.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0];
        section.Assign("LOT-001", 5);

        var service = new StorageService(storage);
        service.ApplyPickedLots(new[] { new PickedLot("LOT-001", 5) });

        Assert.True(section.IsEmpty);
    }

    [Fact]
    public void GetEmptySections_ReturnsAllEmpty()
    {
        var storage = BuildStorage();
        var service = new StorageService(storage);
        // assign one section
        storage.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0].Assign("LOT-001", 1);

        var emptySections = service.GetEmptySections().ToList();
        Assert.DoesNotContain(emptySections, s => s.LotId == "LOT-001");
    }

    [Fact]
    public void GetEmptyContainers_ReturnsContainersWhereAllSectionsAreEmpty()
    {
        var storage = BuildStorage();
        var service = new StorageService(storage);
        // assign a section in the first container
        storage.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0].Assign("LOT-001", 1);

        var emptyContainers = service.GetEmptyContainers().ToList();
        Assert.DoesNotContain(emptyContainers, c => c.Sections.Any(s => s.LotId == "LOT-001"));
    }
}
