namespace LegoStore.Domain;

/// <summary>
/// A cabinet that contains exactly 4 <see cref="CaseGroup"/>s.
/// </summary>
public class Cabinet
{
    public const int GroupCount = 4;

    public IReadOnlyList<CaseGroup> Groups { get; }

    public bool IsEmpty => Groups.All(g => g.IsEmpty);

    public Cabinet(IEnumerable<CaseGroup> groups)
    {
        var list = groups.ToList();
        if (list.Count != GroupCount)
            throw new ArgumentException($"A Cabinet must contain exactly {GroupCount} groups.", nameof(groups));
        Groups = list.AsReadOnly();
    }
}
