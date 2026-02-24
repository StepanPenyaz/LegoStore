namespace LegoStore.Domain;

/// <summary>
/// A physical container that holds one or more <see cref="Section"/>s.
/// The number of sections is determined by its <see cref="ContainerType"/>.
/// </summary>
public class Container
{
    public ContainerType Type { get; }
    public IReadOnlyList<Section> Sections { get; }

    public bool IsEmpty => Sections.All(s => s.IsEmpty);

    public Container(ContainerType type)
    {
        Type = type;
        var sections = new List<Section>(type.SectionCount());
        for (int i = 0; i < type.SectionCount(); i++)
            sections.Add(new Section());
        Sections = sections.AsReadOnly();
    }
}
