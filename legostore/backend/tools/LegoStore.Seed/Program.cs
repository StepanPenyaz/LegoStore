using LegoStore.Domain;
using LegoStore.Infrastructure;
using LegoStore.Infrastructure.Repositories;
using LegoStore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Use same connection string key as API: ConnectionStrings:DefaultConnection
var connectionString =
    configuration.GetConnectionString("DefaultConnection")
    ?? @"Server=(localdb)\MSSQLLocalDB;Database=LegoStore;Trusted_Connection=True;";

var options = new DbContextOptionsBuilder<StorageDbContext>()
    .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(StorageDbContext).Assembly.FullName))
    .Options;

await using var db = new StorageDbContext(options);

// Make sure DB + schema exist
await db.Database.MigrateAsync();

IStorageRepository repo = new StorageRepository(db);


// Build sample storage structure + sample filled sections
// Create 1 Cabinet, 1 CaseGroup, 1 Case (PX12), 1 Container, fill 1 Section
var section = new Section();
section.Assign("LOT123", 100);

var container = new Container(ContainerType.PX12);
// Assign the section to the first slot
var containerSections = container.Sections.ToList();
containerSections[0] = section;
var filledContainer = new Container(ContainerType.PX12);
for (int i = 0; i < filledContainer.Sections.Count; i++)
    filledContainer.Sections[i].Clear();
filledContainer.Sections[0].Assign("LOT123", 100);

var @case = new Case(ContainerType.PX12);
var caseContainers = @case.Containers.ToList();
caseContainers[0] = filledContainer;
var filledCase = new Case(ContainerType.PX12);
for (int i = 0; i < filledCase.Containers.Count; i++)
    foreach (var s in filledCase.Containers[i].Sections)
        s.Clear();
for (int i = 0; i < filledCase.Containers[0].Sections.Count; i++)
    filledCase.Containers[0].Sections[i].Clear();
filledCase.Containers[0].Sections[0].Assign("LOT123", 100);

var cases = Enumerable.Range(0, 9)
    .Select(i => i == 0 ? filledCase : new Case(ContainerType.PX12))
    .ToList();
var group = new CaseGroup(cases);
var groups = Enumerable.Range(0, 4)
    .Select(i => i == 0 ? group : new CaseGroup(Enumerable.Range(0, 9).Select(_ => new Case(ContainerType.PX12))))
    .ToList();
var cabinet = new Cabinet(groups);
var storage = new StoreStorage(new[] { cabinet });

// Persist (StorageRepository.SaveAsync replaces existing storage snapshot)
await repo.SaveAsync(storage);

Console.WriteLine("Seed complete: storage snapshot saved.");