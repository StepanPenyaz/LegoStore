using LegoStore.Domain;

namespace LegoStore.Infrastructure.Entities;

public class CaseEntity
{
    public int Id { get; set; }
    public int CaseGroupId { get; set; }
    public CaseGroupEntity CaseGroup { get; set; } = null!;
    public ContainerType ContainerType { get; set; }
    public ICollection<ContainerEntity> Containers { get; set; } = new List<ContainerEntity>();
}
