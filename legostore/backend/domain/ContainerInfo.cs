namespace LegoStore.Domain;

/// <summary>
/// Describes which container a BSX lot item belongs to and how many sections it occupies.
/// Extracted from the &lt;Remarks&gt; field using the format <c>#NNNN</c> or <c>#NNNN#M</c>.
/// </summary>
public record ContainerInfo(int ContainerNumber, int OccupiedSections);
