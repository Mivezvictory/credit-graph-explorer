namespace CreditGraph.Domain.AlbumModels;

/// <summary>
/// Simplified album with a pre-parsed DateTime to allow stable chronological sorting,
/// regardless of Spotify's varying precision (year/month/day).
/// </summary>
public sealed class AlbumSimplified
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string ReleaseDate { get; set; } = "";
    public string? ReleaseDatePrecision { get; set; }
    public string? LeadtrackUri { get; set; } = null;

    public DateTime ParsedReleaseDate { get; private set; } = DateTime.MinValue;
    public void ComputeParsedDate()
    {
        // Spotify may give "YYYY", "YYYY-MM", or "YYYY-MM-DD"
        var s = ReleaseDate ?? "";
        try
        {
            if (string.Equals(ReleaseDatePrecision, "day", StringComparison.OrdinalIgnoreCase))
                ParsedReleaseDate = DateTime.Parse(s);
            else if (string.Equals(ReleaseDatePrecision, "month", StringComparison.OrdinalIgnoreCase))
                ParsedReleaseDate = DateTime.Parse($"{s}-01");
            else
                ParsedReleaseDate = DateTime.Parse($"{s}-01-01");
        }
        catch { ParsedReleaseDate = DateTime.MinValue; }
    }
    public static AlbumSimplified From(AlbumItem a) => new AlbumSimplified
    {
        Id = a.Id ?? "",
        Name = a.Name ?? "",
        ReleaseDate = a.ReleaseDate ?? "",
        ReleaseDatePrecision = a.ReleaseDatePrecision
    };
}