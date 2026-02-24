using LegoStore.Domain;

namespace LegoStore.Infrastructure.Entities;

public class ContainerEntity
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public CaseEntity Case { get; set; } = null!;
    public ContainerType Type { get; set; }
    public ICollection<SectionEntity> Sections { get; set; } = new List<SectionEntity>();
}
