using System.Text.Json.Serialization;

namespace CreditGraph.Infrastructure.Spotify.DTOs;

public sealed class SpotifyCreatePlaylistDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }

}