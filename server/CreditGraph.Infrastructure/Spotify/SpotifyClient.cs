using System.Text.Json;
using CreditGraph.Services.Exceptions;
using CreditGraph.Services.Interfaces;
using CreditGraph.Infrastructure.Spotify.DTOs;
using CreditGraph.Domain;
using CreditGraph.Domain.AlbumModels;
using System.Text;
using System.Net;
using CreditGraph.Infrastructure.Spotify.QueryObjects;
using CreditGraph.Services.Contracts;
using CreditGraph.Infrastructure.Http;



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
        // var api = await GetSpotifyResponseJson<SpotifyUserDto>(url!, token, "get", ct);
        // if (api.IsSuccess)
        // {
            
        // }
        //     return api.Value.Id;
        // if (string.IsNullOrEmpty(userInfo.Id))
        //     throw new SpotifyProfileInvalidException("Missing 'id' in Spotify /v1/me response.");
        // return userInfo.Id;
        var api = await GetSpotifyResponseJson<SpotifyUserDto>(url!, token, "get", ct);
        if ((int)api.StatusCode >= 400)
            throw new SpotifyProfileInvalidException(
            JsonSerializer.Serialize(new { error = new ErrorShape( 404,$"Missing 'id' in Spotify /v1/me response.") }));

        return api.Value!.Id!;
    }

    public async Task<Artist> GetArtistAsync(
        string artistId,
        string token,
        CancellationToken ct = default
    )
    {
        var url = $"artists/{artistId}";

        //while (!string.IsNullOrEmpty(url))
        var api = await GetSpotifyResponseJson<Artist>(url!, token, "get", ct);

        //we will always have a valid spotify id 
        //we are handling getting the id on the frontend (we lol, its just me here. Someone is starting to lose it)
        //so invalid artist search will be handled on the frontend

        //any non successful calls will be handled by GetSpotifyResponseJson
        //along with null 
        if (!api.IsSuccess)
        {
            if ((int)api.StatusCode == 404)
                throw new SpotifyArtistNotFoundException(CreateErrorMessage(404, $"artist not found for id: {artistId}"));
        }
        if(api.Value is null)
            throw new SpotifyArtistNotFoundException(CreateErrorMessage(404, $"artist not found for id: {artistId}"));

        return api.Value!;
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
            var api = await GetSpotifyResponseJson<AlbumPage>(url!, token, "get", ct);

            if (!api.IsSuccess)
            {
                if ((int)api.StatusCode == 404)
                    throw new SpotifyArtistNotFoundException(CreateErrorMessage(404, $"Artist not found: {artistId}"));
            }
            var root = api.Value;
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
        var api = await GetSpotifyResponseJson<SpotifyCreatePlaylistDto>(url!, token, "post", ct, jsonString);
        if (!api.IsSuccess)
            throw new SpotifyPlaylistException(CreateErrorMessage(400, $"Attempts to create {playlistName} Playlist failed."));
        
        return api.Value!.Id!;
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
    private async Task<ApiResult<T>> GetSpotifyResponseJson<T>(string url, string token, string method, CancellationToken ct, string? reqBody = null)
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
        var api = await res.ToApiResultAsync<T>(ct).ConfigureAwait(false);

        switch (res.StatusCode)
        {
            case HttpStatusCode.Unauthorized:   throw new SpotifyUnauthorizedException(CreateErrorMessage(401, $"Token invalid/expired."));
            case HttpStatusCode.Forbidden:      throw new SpotifyScopeException(CreateErrorMessage(403, $"Missing scope."));
            case HttpStatusCode.TooManyRequests:           throw new SpotifyRateLimitedException(api.RetryAfter, CreateErrorMessage(429, $"Rate limited"));
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.ServiceUnavailable:
            case HttpStatusCode.GatewayTimeout: throw new SpotifyUnavailableException(CreateErrorMessage(503, "Spotify unavailable."));
        }

        // Everything else (incl. 200/201/204, and 400/404/409/422…) goes back to caller
        return api;
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

    /// <summary>
    /// Create a serialized error message of type ErrorShape
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="errorMessage"></param>
    /// <returns>A string of an ErrorShape object</returns>
    private static string CreateErrorMessage(int statusCode, string errorMessage)
    {
        return JsonSerializer.Serialize(new { error = new ErrorShape(statusCode, errorMessage) });
    }
    
}