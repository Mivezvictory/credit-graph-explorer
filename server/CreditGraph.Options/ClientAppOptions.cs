namespace CreditGraph.Options;

/// <summary>
/// Typed configuration for the front-end base URL .
/// </summary>
public class ClientAppOptions
{
    /// <summary>SPA origin/base URL (e.g., http://localhost:5173).</summary>
    public string BaseUrl { get; set; } = "http://localhost:5173";
}
