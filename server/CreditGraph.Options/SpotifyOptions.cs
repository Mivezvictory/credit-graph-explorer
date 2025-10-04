namespace CreditGraph.Options;

/// <summary>
/// Typed configuration for Spotify OAuth + Web API.
/// Bound from configuration section "Spotify".
/// </summary>
public class SpotifyOptions
{
    /// <summary>Spotify app client id.</summary>
    public string ClientId { get; set; } = "";

    /// <summary>Spotify app client secret (store securely in prod).</summary>
    public string ClientSecret { get; set; } = "";

    /// <summary>
    /// OAuth redirect URI registered in the Spotify dashboard.
    /// </summary>
    public string RedirectUri { get; set; } = "";
}
