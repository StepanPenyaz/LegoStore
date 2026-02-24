using LegoStore.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace LegoStore.Infrastructure;

/// <summary>
/// EF Core database context for LegoStore SQL Server Express persistence.
/// </summary>
public class StorageDbContext : DbContext
{
    public DbSet<StorageEntity> Storages { get; set; }
    public DbSet<CabinetEntity> Cabinets { get; set; }
    public DbSet<CaseGroupEntity> CaseGroups { get; set; }
    public DbSet<CaseEntity> Cases { get; set; }
    public DbSet<ContainerEntity> Containers { get; set; }
    public DbSet<SectionEntity> Sections { get; set; }

    public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageEntity>(e =>
        {
            e.ToTable("Storages");
            e.HasKey(s => s.Id);
            e.HasMany(s => s.Cabinets)
             .WithOne(c => c.Storage)
             .HasForeignKey(c => c.StorageId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CabinetEntity>(e =>
        {
            e.ToTable("Cabinets");
            e.HasKey(c => c.Id);
            e.HasMany(c => c.Groups)
             .WithOne(g => g.Cabinet)
             .HasForeignKey(g => g.CabinetId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CaseGroupEntity>(e =>
        {
            e.ToTable("CaseGroups");
            e.HasKey(g => g.Id);
            e.HasMany(g => g.Cases)
             .WithOne(c => c.CaseGroup)
             .HasForeignKey(c => c.CaseGroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CaseEntity>(e =>
        {
            e.ToTable("Cases");
            e.HasKey(c => c.Id);
            e.Property(c => c.ContainerType).HasConversion<string>();
            e.HasMany(c => c.Containers)
             .WithOne(con => con.Case)
             .HasForeignKey(con => con.CaseId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ContainerEntity>(e =>
        {
            e.ToTable("Containers");
            e.HasKey(c => c.Id);
            e.Property(c => c.Type).HasConversion<string>();
            e.HasMany(c => c.Sections)
             .WithOne(s => s.Container)
             .HasForeignKey(s => s.ContainerId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SectionEntity>(e =>
        {
            e.ToTable("Sections");
            e.HasKey(s => s.Id);
            e.Property(s => s.LotId).HasMaxLength(100);
        });
    }
}
