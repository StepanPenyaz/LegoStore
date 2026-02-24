namespace LegoStore.Infrastructure.Entities;

public class SectionEntity
{
    public int Id { get; set; }
    public int ContainerId { get; set; }
    public ContainerEntity Container { get; set; } = null!;
    public string? LotId { get; set; }
    public int Quantity { get; set; }
}
