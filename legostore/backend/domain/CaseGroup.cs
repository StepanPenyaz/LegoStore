namespace LegoStore.Domain;

/// <summary>
/// A 3×3 grid of <see cref="Case"/>s (9 cases per group).
/// </summary>
public class CaseGroup
{
    public const int Rows = 3;
    public const int Columns = 3;
    public const int CaseCount = Rows * Columns;

    public IReadOnlyList<Case> Cases { get; }

    public bool IsEmpty => Cases.All(c => c.IsEmpty);

    public CaseGroup(IEnumerable<Case> cases)
    {
        var list = cases.ToList();
        if (list.Count != CaseCount)
            throw new ArgumentException($"A CaseGroup must contain exactly {CaseCount} cases.", nameof(cases));
        Cases = list.AsReadOnly();
    }
}
