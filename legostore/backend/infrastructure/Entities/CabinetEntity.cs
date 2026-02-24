namespace LegoStore.Infrastructure.Entities;

public class CabinetEntity
{
    public int Id { get; set; }
    public int StorageId { get; set; }
    public StorageEntity Storage { get; set; } = null!;
    public ICollection<CaseGroupEntity> Groups { get; set; } = new List<CaseGroupEntity>();
}
