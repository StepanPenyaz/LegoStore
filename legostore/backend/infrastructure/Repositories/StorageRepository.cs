using LegoStore.Domain;
using LegoStore.Infrastructure.Entities;
using LegoStore.Services;
using Microsoft.EntityFrameworkCore;

namespace LegoStore.Infrastructure.Repositories;

/// <inheritdoc cref="IStorageRepository"/>
public class StorageRepository : IStorageRepository
{
    private readonly StorageDbContext _db;

    public StorageRepository(StorageDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<StoreStorage?> LoadAsync(CancellationToken ct = default)
    {
        var entity = await _db.Storages
            .Include(s => s.Cabinets)
                .ThenInclude(c => c.Groups)
                    .ThenInclude(g => g.Cases)
                        .ThenInclude(c => c.Containers)
                            .ThenInclude(con => con.Sections)
            .FirstOrDefaultAsync(ct);

        return entity is null ? null : MapToDomain(entity);
    }

    /// <inheritdoc/>
    public async Task SaveAsync(StoreStorage storage, CancellationToken ct = default)
    {
        var existing = await _db.Storages.FirstOrDefaultAsync(ct);
        if (existing is not null)
        {
            _db.Storages.Remove(existing);
            await _db.SaveChangesAsync(ct);
        }

        _db.Storages.Add(MapToEntity(storage));
        await _db.SaveChangesAsync(ct);
    }

    // ── Mapping: domain → entity ─────────────────────────────────────────────

    private static StorageEntity MapToEntity(StoreStorage storage) =>
        new()
        {
            Cabinets = storage.Cabinets.Select(MapCabinetToEntity).ToList()
        };

    private static CabinetEntity MapCabinetToEntity(Cabinet cabinet) =>
        new()
        {
            Groups = cabinet.Groups.Select(MapGroupToEntity).ToList()
        };

    private static CaseGroupEntity MapGroupToEntity(CaseGroup group) =>
        new()
        {
            Cases = group.Cases.Select(MapCaseToEntity).ToList()
        };

    private static CaseEntity MapCaseToEntity(Case @case) =>
        new()
        {
            ContainerType = @case.ContainerType,
            Containers    = @case.Containers.Select(MapContainerToEntity).ToList()
        };

    private static ContainerEntity MapContainerToEntity(Container container) =>
        new()
        {
            Type     = container.Type,
            Sections = container.Sections.Select(MapSectionToEntity).ToList()
        };

    private static SectionEntity MapSectionToEntity(Section section) =>
        new()
        {
            LotId    = section.LotId,
            Quantity = section.Quantity
        };

    // ── Mapping: entity → domain ─────────────────────────────────────────────

    private static StoreStorage MapToDomain(StorageEntity entity) =>
        new(entity.Cabinets.Select(MapCabinetToDomain));

    private static Cabinet MapCabinetToDomain(CabinetEntity entity) =>
        new(entity.Groups.Select(MapGroupToDomain));

    private static CaseGroup MapGroupToDomain(CaseGroupEntity entity) =>
        new(entity.Cases.Select(MapCaseToDomain));

    private static Case MapCaseToDomain(CaseEntity entity)
    {
        var @case = new Case(entity.ContainerType);
        // Restore section state from persisted data
        for (int i = 0; i < entity.Containers.Count; i++)
        {
            var containerEntity = entity.Containers.ElementAt(i);
            var container       = @case.Containers[i];
            for (int j = 0; j < containerEntity.Sections.Count; j++)
            {
                var sectionEntity = containerEntity.Sections.ElementAt(j);
                var section       = container.Sections[j];
                if (sectionEntity.LotId is not null)
                    section.Assign(sectionEntity.LotId, sectionEntity.Quantity);
            }
        }
        return @case;
    }
}
