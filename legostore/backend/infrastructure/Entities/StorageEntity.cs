namespace LegoStore.Infrastructure.Entities;

public class StorageEntity
{
    public int Id { get; set; }
    public ICollection<CabinetEntity> Cabinets { get; set; } = new List<CabinetEntity>();
}
