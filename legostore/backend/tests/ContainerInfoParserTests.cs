using LegoStore.Domain;
using LegoStore.Services;

namespace LegoStore.Tests;

public class ContainerInfoParserTests
{
    [Theory]
    [InlineData("#1012#2", 1012, 2)]
    [InlineData("#2122#1", 2122, 1)]
    [InlineData("#5",      5,    1)]
    public void TryParse_ValidRemarks_ReturnsContainerInfo(
        string remarks, int expectedNumber, int expectedSections)
    {
        var result = ContainerInfoParser.TryParse(remarks);

        Assert.NotNull(result);
        Assert.Equal(expectedNumber, result!.ContainerNumber);
        Assert.Equal(expectedSections, result.OccupiedSections);
    }

    [Fact]
    public void TryParse_SingleHash_DefaultsToOneSection()
    {
        var result = ContainerInfoParser.TryParse("#2122");

        Assert.NotNull(result);
        Assert.Equal(2122, result!.ContainerNumber);
        Assert.Equal(1, result.OccupiedSections);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("no hash")]
    [InlineData("#abc")]
    [InlineData("#12#abc")]
    public void TryParse_InvalidOrMissingRemarks_ReturnsNull(string? remarks)
    {
        var result = ContainerInfoParser.TryParse(remarks);

        Assert.Null(result);
    }
}
