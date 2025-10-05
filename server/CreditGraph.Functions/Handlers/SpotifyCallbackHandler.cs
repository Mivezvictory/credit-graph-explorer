using Microsoft.AspNetCore.Http;
using CreditGraph.Domain.Exceptions;
using CreditGraph.Domain.Models.Spotify;
using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using CreditGraph.Options;

namespace CreditGraph.Functions.Handlers;

public interface ISpotifyCallbackHandler
{
    string GetAuthCode(HttpRequest req);
    Task<SpotifyTokenResponse?> ExchangeCodeForTokensAsync(string code, SpotifyOptions spotify);
    string CombineUrl(string baseUrl, string path);
}
public class SpotifyCallbackHandler : ISpotifyCallbackHandler
{
    /// <summary>
    /// Retrieves either an error message or an Authorization Code. Throws an exception if error message retrieved
    /// </summary>
    /// <param name="req">The query request</param>
    /// <returns>An Authorization code</returns>
    /// <exception cref="SpotifyCallbackException"></exception>
    public String GetAuthCode(HttpRequest req)
    {

        var error = req.Query["error"];
        if (!string.IsNullOrEmpty(error))
            throw new SpotifyCallbackException($"Spotify error: {error}");

        var code = req.Query["code"];
        if (string.IsNullOrWhiteSpace(code))
            throw new SpotifyCallbackException("Missing authorization code.");
        return code!;
    }

    /// <summary>
    /// Exchange spotify authentication code for authentication token
    /// </summary>
    /// <param name="code">The authentication code</param>
    /// <param name="spotify">The SpotifyOptions object containing all spotify related environment variables</param>
    /// <returns cref="SpotifyTokenResponse">A SpotifyTokenResponse object</returns>
    /// <exception cref="Exception"></exception>
    public async Task<SpotifyTokenResponse?> ExchangeCodeForTokensAsync(string code, SpotifyOptions spotify)
    {
        var body = new StringBuilder()
            .Append("grant_type=authorization_code")
            .Append("&code=").Append(Uri.EscapeDataString(code))
            .Append("&redirect_uri=").Append(Uri.EscapeDataString(spotify.RedirectUri))
            .ToString();

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        req.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
        req.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{spotify.ClientId}:{spotify.ClientSecret}"))
        );
        req.Headers.Accept.ParseAdd("application/json");
        HttpClient _http = new HttpClient();
        using var res = await _http.SendAsync(req);
        var json = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
        {
            throw new Exception($"Token exchange failed: {res.StatusCode} {json}");
        }

        try
        {
            return JsonSerializer.Deserialize<SpotifyTokenResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// A helper method that combines the baseUrl with the provided path
    /// </summary>
    /// <param name="baseUrl">The baseUrl</param>
    /// <param name="path">The intended path to be appended to the baseUrl</param>
    /// <returns>A string of the combined baseUrl+path</returns>
    public string CombineUrl(string baseUrl, string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return baseUrl;
        if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return path;
        return baseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
    }


}