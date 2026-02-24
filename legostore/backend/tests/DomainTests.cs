using LegoStore.Domain;

namespace LegoStore.Tests;

public class ContainerTypeTests
{
    [Theory]
    [InlineData(ContainerType.PX12, 3)]
    [InlineData(ContainerType.PX6,  1)]
    [InlineData(ContainerType.PX4,  1)]
    [InlineData(ContainerType.PX2,  1)]
    public void SectionCount_ReturnsCorrectCount(ContainerType type, int expectedSections)
    {
        Assert.Equal(expectedSections, type.SectionCount());
    }

    [Theory]
    [InlineData(ContainerType.PX12, 12)]
    [InlineData(ContainerType.PX6,   6)]
    [InlineData(ContainerType.PX4,   4)]
    [InlineData(ContainerType.PX2,   2)]
    public void ContainersPerCase_ReturnsCorrectCount(ContainerType type, int expectedContainers)
    {
        Assert.Equal(expectedContainers, type.ContainersPerCase());
    }
}

public class ContainerTests
{
    [Theory]
    [InlineData(ContainerType.PX12, 3)]
    [InlineData(ContainerType.PX6,  1)]
    [InlineData(ContainerType.PX4,  1)]
    [InlineData(ContainerType.PX2,  1)]
    public void Container_HasCorrectNumberOfSections(ContainerType type, int expectedSections)
    {
        var container = new Container(type);
        Assert.Equal(expectedSections, container.Sections.Count);
    }

    [Fact]
    public void NewContainer_IsEmpty()
    {
        var container = new Container(ContainerType.PX12);
        Assert.True(container.IsEmpty);
    }
}

public class CaseTests
{
    [Theory]
    [InlineData(ContainerType.PX12, 12)]
    [InlineData(ContainerType.PX6,   6)]
    [InlineData(ContainerType.PX4,   4)]
    [InlineData(ContainerType.PX2,   2)]
    public void Case_HasCorrectNumberOfContainers(ContainerType type, int expectedContainers)
    {
        var c = new Case(type);
        Assert.Equal(expectedContainers, c.Containers.Count);
    }

    [Fact]
    public void NewCase_IsEmpty()
    {
        var c = new Case(ContainerType.PX6);
        Assert.True(c.IsEmpty);
    }
}

public class CaseGroupTests
{
    [Fact]
    public void CaseGroup_RequiresExactlyNineCases()
    {
        var cases = Enumerable.Range(0, 9).Select(_ => new Case(ContainerType.PX6)).ToList();
        var group = new CaseGroup(cases);
        Assert.Equal(9, group.Cases.Count);
    }

    [Fact]
    public void CaseGroup_WrongCount_Throws()
    {
        var cases = Enumerable.Range(0, 5).Select(_ => new Case(ContainerType.PX6)).ToList();
        Assert.Throws<ArgumentException>(() => new CaseGroup(cases));
    }
}

public class CabinetTests
{
    private static CaseGroup MakeGroup() =>
        new CaseGroup(Enumerable.Range(0, 9).Select(_ => new Case(ContainerType.PX6)));

    [Fact]
    public void Cabinet_RequiresExactlyFourGroups()
    {
        var groups = Enumerable.Range(0, 4).Select(_ => MakeGroup()).ToList();
        var cabinet = new Cabinet(groups);
        Assert.Equal(4, cabinet.Groups.Count);
    }

    [Fact]
    public void Cabinet_WrongCount_Throws()
    {
        var groups = Enumerable.Range(0, 3).Select(_ => MakeGroup()).ToList();
        Assert.Throws<ArgumentException>(() => new Cabinet(groups));
    }
}

public class StoreStorageTests
{
    private static CaseGroup MakeGroup() =>
        new CaseGroup(Enumerable.Range(0, 9).Select(_ => new Case(ContainerType.PX6)));

    private static StoreStorage BuildStorage()
    {
        var cabinet = new Cabinet(Enumerable.Range(0, 4).Select(_ => MakeGroup()));
        return new StoreStorage(new[] { cabinet });
    }

    [Fact]
    public void FindSectionsByLotId_FindsAssignedSection()
    {
        var storage = BuildStorage();
        var section = storage.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0];
        section.Assign("LOT-ABC", 20);

        var found = storage.FindSectionsByLotId("LOT-ABC").ToList();
        Assert.Single(found);
        Assert.Equal(20, found[0].Quantity);
    }

    [Fact]
    public void FindSectionsByLotId_ReturnsEmpty_WhenNotFound()
    {
        var storage = BuildStorage();
        Assert.Empty(storage.FindSectionsByLotId("NONEXISTENT"));
    }
}
