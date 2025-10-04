namespace CreditGraph.Options;

/// <summary>
/// Typed configuration for MusicBrainz API.
/// MusicBrainz requires a meaningful User-Agent; we bind it here once
/// and apply it to the typed HttpClient so every request is compliant.
/// </summary>
public class MusicBrainzOptions
{
    /// <summary>
    /// Required: “AppName/Version (+URL; contact@email)”.
    /// Used to identify your app to MusicBrainz politely.
    /// </summary>
    public string UserAgent { get; set; } = "CreditGraphExplorer/0.1 (+http://localhost; dev@example.com)";

    /// <summary>Base URL for MusicBrainz WS API.</summary>
    public string BaseUrl { get; set; } = "https://musicbrainz.org/ws/2/";
}
