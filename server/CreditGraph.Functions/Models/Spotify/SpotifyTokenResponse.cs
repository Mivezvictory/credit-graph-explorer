using System.Text.Json.Serialization;
namespace CreditGraph.Functions.Models.Spotify;
/// <summary>Spotify token payload (snake_case mapped to C#).</summary>
public sealed class SpotifyTokenResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
    [JsonPropertyName("token_type")] public string TokenType { get; set; } = "";
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
    [JsonPropertyName("refresh_token")] public string? RefreshToken { get; set; }
    [JsonPropertyName("scope")] public string Scope { get; set; } = "";
}