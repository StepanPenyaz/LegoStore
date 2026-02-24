using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LegoStore.Infrastructure;

/// <summary>
/// Design-time factory used by EF Core tooling (e.g. dotnet-ef migrations).
/// Uses a placeholder SQL Server Express connection string for scaffolding only.
/// </summary>
public class StorageDbContextFactory : IDesignTimeDbContextFactory<StorageDbContext>
{
    public StorageDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<StorageDbContext>()
            .UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=LegoStore;Trusted_Connection=True;",
                sql => sql.MigrationsAssembly(typeof(StorageDbContext).Assembly.FullName))
            .Options;

        return new StorageDbContext(options);
    }
}
