namespace LegoStore.Infrastructure.Entities;

public class CaseGroupEntity
{
    public int Id { get; set; }
    public int CabinetId { get; set; }
    public CabinetEntity Cabinet { get; set; } = null!;
    public ICollection<CaseEntity> Cases { get; set; } = new List<CaseEntity>();
}
