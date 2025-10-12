using System.Text.Json.Serialization;

namespace CreditGraph.Infrastructure.Spotify.DTOs;

public sealed class SpotifyUserDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }

}