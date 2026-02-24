using System.Text.RegularExpressions;
using LegoStore.Domain;

namespace LegoStore.Services;

/// <summary>
/// Parses the <c>Remarks</c> field of a BSX item to extract container location info.
/// </summary>
/// <remarks>
/// Supported formats:
/// <list type="bullet">
///   <item><c>#1012#2</c> → ContainerNumber=1012, OccupiedSections=2</item>
///   <item><c>#2122</c>   → ContainerNumber=2122, OccupiedSections=1</item>
/// </list>
/// </remarks>
public static class ContainerInfoParser
{
    // Matches "#NNN" or "#NNN#M" where NNN and M are integers.
    private static readonly Regex Pattern =
        new(@"^#(\d+)(?:#(\d+))?$", RegexOptions.Compiled);

    /// <summary>
    /// Tries to parse container information from the given <paramref name="remarks"/> string.
    /// Returns <c>null</c> when the string is null, empty, or does not match the expected format.
    /// </summary>
    public static ContainerInfo? TryParse(string? remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks))
            return null;

        var match = Pattern.Match(remarks.Trim());
        if (!match.Success)
            return null;

        int containerNumber = int.Parse(match.Groups[1].Value);
        int occupiedSections = match.Groups[2].Success
            ? int.Parse(match.Groups[2].Value)
            : 1;

        return new ContainerInfo(containerNumber, occupiedSections);
    }
}
