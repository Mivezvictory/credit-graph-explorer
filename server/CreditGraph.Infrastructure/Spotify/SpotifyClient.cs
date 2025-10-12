using System.Text.Json;
using CreditGraph.Services.Exceptions;
using CreditGraph.Services.Interfaces;
using CreditGraph.Infrastructure.Spotify.DTOs;
using CreditGraph.Domain;
using CreditGraph.Domain.AlbumModels;
using System.Text;
using CreditGraph.Infrastructure.Spotify.QueryObjects;


namespace CreditGraph.Infrastructure.Spotify;

/// <summary>
/// Default implementation of <see cref="ISpotifyClient"/> backed by a typed HttpClient.
/// </summary>
public class SpotifyClient : ISpotifyClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public SpotifyClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> GetCurrentUserIdAsync(
        string token,
        CancellationToken ct = default
    )
    {
        var url = $"me";
        //while (!string.IsNullOrEmpty(url))
        var userInfo = await GetSpotifyResponseJson<SpotifyUserDto>(url!, token, "get", ct);
        if (string.IsNullOrEmpty(userInfo.Id))
            throw new SpotifyProfileInvalidException("Missing 'id' in Spotify /v1/me response.");
        return userInfo.Id;
    }

    public async Task<Artist> GetArtistAsync(
        string artistId,
        string token,
        CancellationToken ct = default
    )
    {
        var url = $"artists/{artistId}";

        //while (!string.IsNullOrEmpty(url))
        var artist = await GetSpotifyResponseJson<Artist>(url!, token, "get", ct);

        //we will always have a valid spotify id 
        //we are handling getting the id on the frontend (we lol, its just me here. Someone is starting to lose it)
        //so invalid artist search will be handled on the frontend

        //any non successful calls will be handled by GetSpotifyResponseJson
        //along with null 
        return artist;
    }

    public async Task<List<AlbumSimplified>> GetArtistAlbumsAsync(
        string artistId,
        string token,
        int limit,
        string market,
        IEnumerable<string> includeGroups,
        CancellationToken ct = default

    )
    {
        var results = new List<AlbumSimplified>();
        var groups = string.Join(",", includeGroups);
        var url = $"artists/{artistId}/albums?include_groups={Uri.EscapeDataString(groups)}&market={Uri.EscapeDataString(market)}&limit={limit}&offset=0";

        //Can ComputeParsedDate be done within the while loop for more efficiency?
        while (!string.IsNullOrEmpty(url))
        {
            var root = await GetSpotifyResponseJson<AlbumPage>(url!, token, "get", ct);
            if (root?.Items != null)
            {
                results.AddRange(root.Items.Select(AlbumSimplified.From));
                url = root.Next;
            }
        }
        foreach (var a in results)
            a.ComputeParsedDate();


        return results;
    }

    public async Task<string> CreatePlaylistAsync(
        string userId,
        string playlistName,
        string token,
        bool isPublic,
        string description,
        CancellationToken ct = default
    )
    {
        var url = $"users/{userId}/playlists";
        var queryBody = new SpotifyPlaylistQueryObj
        (
            playlistName,
            description,
            isPublic
        );
        var jsonString = JsonSerializer.Serialize(queryBody);
        var playlist = await GetSpotifyResponseJson<SpotifyCreatePlaylistDto>(url!, token, "post", ct, jsonString);
        if (string.IsNullOrEmpty(playlist.Id))
            throw new SpotifyPlaylistException($"Attempts to create {playlistName} Playlist failed.");
        return playlist.Id;
    }

    public async Task AddTracksToPlaylistAsync(
        string playlistId,
        List<string> trackUris,
        string token,
        CancellationToken ct = default

    )
    {
        var url = $"playlist/{playlistId}/tracks";
        var queryBody = new SpotifyAddTracksToPlaylistQueryObject
        (
            trackUris
        );
        var jsonString = JsonSerializer.Serialize(queryBody);
        await GetSpotifyResponseJson<SpotifyCreatePlaylistDto>(url!, token, "post", ct, jsonString);
    }


    /// <summary>
    /// Handles sending/receiving and parsing spotify requests
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="token"></param>
    /// <param name="method"></param>
    /// <param name="httpContent">(Optional) serialized request body</param> 
    /// <returns>Generic type of spotify response JSON as defined by caller</returns>
    /// <exception cref="Exception"></exception>
    private async Task<T> GetSpotifyResponseJson<T>(string url, string token, string method, CancellationToken ct, string? reqBody = null)
    {
        using var req = new HttpRequestMessage(GetHttpMethod(method), url);
        if (!string.IsNullOrEmpty(reqBody))
        {
            var httpContent = new StringContent(reqBody, Encoding.UTF8, "application/json");
            req.Content = httpContent;
        }

        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer",
            token
        );

        using var res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
        //before we check if the status code is successful, I also want to check for 3 (Spotify) specific status codes
        //401, 403, 429

        if ((int)res.StatusCode == 401 || (int)res.StatusCode == 403)
            throw new SpotifyUnauthorizedException();
        if ((int)res.StatusCode == 429)
            throw new SpotifyRateLimitedException();
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(body, _json);

        return result!;

    }

    /// <summary>
    /// Overload method to handle calls where no return obj is excpected
    /// </summary>
    /// <param name="url"></param>
    /// <param name="token"></param>
    /// <param name="method"></param>
    /// <param name="ct"></param>
    /// <param name="reqBody"></param>
    /// <returns></returns>
    /// <exception cref="SpotifyUnauthorizedException"></exception>
    /// <exception cref="SpotifyRateLimitedException"></exception>
    private async Task GetSpotifyResponseJson(string url, string token, string method, CancellationToken ct, string? reqBody = null)
    {
        using var req = new HttpRequestMessage(GetHttpMethod(method), url);
        if (!string.IsNullOrEmpty(reqBody))
        {
            var httpContent = new StringContent(reqBody, Encoding.UTF8, "application/json");
            req.Content = httpContent;
        }

        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer",
            token
        );

        using var res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
        //before we check if the status code is successful, I also want to check for 3 (Spotify) specific status codes
        //401, 403, 429

        if ((int)res.StatusCode == 401 || (int)res.StatusCode == 403)
            throw new SpotifyUnauthorizedException();
        if ((int)res.StatusCode == 429)
            throw new SpotifyRateLimitedException();
        res.EnsureSuccessStatusCode();
    }


    /// <summary>
    /// Returns Httpmethod matching provided method parameter
    /// </summary>
    /// <param name="method"></param>
    /// <returns>Httpmethod or throw exception provided method parameter is invalid</returns>
    /// <exception cref="Exception"></exception>
    private static HttpMethod GetHttpMethod(string method)
    {
        return method.ToLower() switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
            _ => throw new Exception($"Invalid method, {method}, was provided")
        };
    }
    
}