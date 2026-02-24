using LegoStore.Domain;
using LegoStore.Infrastructure;
using LegoStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LegoStore.Tests;

public class StorageRepositoryTests
{
    private static StorageDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<StorageDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new StorageDbContext(options);
    }

    private static StoreStorage BuildStorage()
    {
        var group   = new CaseGroup(Enumerable.Range(0, 9).Select(_ => new Case(ContainerType.PX6)));
        var cabinet = new Cabinet(Enumerable.Range(0, 4).Select(_ => group));
        return new StoreStorage(new[] { cabinet });
    }

    [Fact]
    public async Task LoadAsync_ReturnsNull_WhenNothingSaved()
    {
        await using var db = CreateInMemoryContext();
        var repo = new StorageRepository(db);

        var result = await repo.LoadAsync();

        Assert.Null(result);
    }

    [Fact]
    public async Task SaveAsync_ThenLoadAsync_ReturnsSameStructure()
    {
        await using var db = CreateInMemoryContext();
        var repo    = new StorageRepository(db);
        var storage = BuildStorage();

        await repo.SaveAsync(storage);
        var loaded = await repo.LoadAsync();

        Assert.NotNull(loaded);
        Assert.Single(loaded.Cabinets);
        Assert.Equal(4, loaded.Cabinets[0].Groups.Count);
        Assert.Equal(9, loaded.Cabinets[0].Groups[0].Cases.Count);
        Assert.Equal(6, loaded.Cabinets[0].Groups[0].Cases[0].Containers.Count);
    }

    [Fact]
    public async Task SaveAsync_ThenLoadAsync_RestoresSectionState()
    {
        await using var db = CreateInMemoryContext();
        var repo    = new StorageRepository(db);
        var storage = BuildStorage();

        storage.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0].Assign("LOT-001", 42);

        await repo.SaveAsync(storage);
        var loaded = await repo.LoadAsync();

        var section = loaded!.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0];
        Assert.Equal("LOT-001", section.LotId);
        Assert.Equal(42, section.Quantity);
    }

    [Fact]
    public async Task SaveAsync_ReplacesExistingStorage()
    {
        await using var db = CreateInMemoryContext();
        var repo = new StorageRepository(db);

        var first = BuildStorage();
        first.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0].Assign("OLD", 5);
        await repo.SaveAsync(first);

        var second = BuildStorage();
        second.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0].Assign("NEW", 99);
        await repo.SaveAsync(second);

        var loaded = await repo.LoadAsync();
        var section = loaded!.Cabinets[0].Groups[0].Cases[0].Containers[0].Sections[0];
        Assert.Equal("NEW", section.LotId);
        Assert.Equal(99, section.Quantity);

        // Only one storage record should exist
        Assert.Equal(1, await db.Storages.CountAsync());
    }
}
