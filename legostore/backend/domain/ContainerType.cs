namespace LegoStore.Domain;

/// <summary>
/// Defines the container types available and their section capacity.
/// </summary>
public enum ContainerType
{
    PX12 = 12,
    PX6  = 6,
    PX4  = 4,
    PX2  = 2
}

public static class ContainerTypeExtensions
{
    /// <summary>Returns the number of sections a container of this type holds.</summary>
    public static int SectionCount(this ContainerType type) => type switch
    {
        ContainerType.PX12 => 3,
        ContainerType.PX6  => 1,
        ContainerType.PX4  => 1,
        ContainerType.PX2  => 1,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    /// <summary>Returns the number of containers this type allows per Case.</summary>
    public static int ContainersPerCase(this ContainerType type) => type switch
    {
        ContainerType.PX12 => 12,
        ContainerType.PX6  => 6,
        ContainerType.PX4  => 4,
        ContainerType.PX2  => 2,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}
