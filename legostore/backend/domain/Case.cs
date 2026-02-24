namespace LegoStore.Domain;

/// <summary>
/// A physical case that holds containers of a single <see cref="ContainerType"/>.
/// Capacity (number of containers) is determined by the container type.
/// </summary>
public class Case
{
    public ContainerType ContainerType { get; }
    public IReadOnlyList<Container> Containers { get; }

    public bool IsEmpty => Containers.All(c => c.IsEmpty);

    public Case(ContainerType containerType)
    {
        ContainerType = containerType;
        int capacity = containerType.ContainersPerCase();
        var containers = new List<Container>(capacity);
        for (int i = 0; i < capacity; i++)
            containers.Add(new Container(containerType));
        Containers = containers.AsReadOnly();
    }
}
