namespace LegoStore.Api.Dtos;

/// <summary>Represents the full storage state returned by the API.</summary>
public record StorageDto(IReadOnlyList<CabinetDto> Cabinets);

/// <summary>Represents a single cabinet with its case groups.</summary>
public record CabinetDto(int Id, string Name, IReadOnlyList<CaseGroupDto> CaseGroups);

/// <summary>Represents a case group containing containers.</summary>
public record CaseGroupDto(int Id, string Label, string ContainerType, IReadOnlyList<ContainerDto> Containers);

/// <summary>Represents a single container with its section state.</summary>
public record ContainerDto(int Id, int TotalSections, int EmptySections, IReadOnlyList<SectionDto> Sections);

/// <summary>Represents a single section inside a container.</summary>
public record SectionDto(int Index, bool IsEmpty, string? LotId, int Quantity);
