using System.Text.Json;
using CreditGraph.Services.Interfaces;
namespace CreditGraph.Services.Implentations;

/// <summary>
/// Default implementation of <see cref="ISpotifyClient"/> backed by a typed HttpClient.
/// All Spotify HTTP calls, auth headers, paging, and error logging live here—
/// not in Functions—so other services can reuse them and tests can mock them.
/// </summary>
public class SpotifyClient : ISpotifyClient
{
    private readonly HttpClient _http;

    private readonly JsonSerializerOptions _json = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// The typed HttpClient is configured in Program.cs (base address, Accept header, etc.).
    /// </summary>
    public SpotifyClient(HttpClient http) => _http = http;
}
