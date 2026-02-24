using LegoStore.Domain;

namespace LegoStore.Tests;

public class SectionTests
{
    [Fact]
    public void NewSection_IsEmpty()
    {
        var section = new Section();
        Assert.True(section.IsEmpty);
        Assert.Null(section.LotId);
        Assert.Equal(0, section.Quantity);
    }

    [Fact]
    public void Assign_SetsLotIdAndQuantity()
    {
        var section = new Section();
        section.Assign("LOT-001", 42);
        Assert.False(section.IsEmpty);
        Assert.Equal("LOT-001", section.LotId);
        Assert.Equal(42, section.Quantity);
    }

    [Fact]
    public void Deduct_ReducesQuantity()
    {
        var section = new Section();
        section.Assign("LOT-001", 10);
        section.Deduct(3);
        Assert.Equal(7, section.Quantity);
    }

    [Fact]
    public void Deduct_ToZero_ClearsSection()
    {
        var section = new Section();
        section.Assign("LOT-001", 5);
        section.Deduct(5);
        Assert.True(section.IsEmpty);
    }

    [Fact]
    public void Deduct_MoreThanQuantity_Throws()
    {
        var section = new Section();
        section.Assign("LOT-001", 5);
        Assert.Throws<InvalidOperationException>(() => section.Deduct(10));
    }

    [Fact]
    public void Assign_EmptyLotId_Throws()
    {
        var section = new Section();
        Assert.Throws<ArgumentException>(() => section.Assign("", 5));
    }

    [Fact]
    public void Clear_ResetsSection()
    {
        var section = new Section();
        section.Assign("LOT-001", 10);
        section.Clear();
        Assert.True(section.IsEmpty);
    }
}
