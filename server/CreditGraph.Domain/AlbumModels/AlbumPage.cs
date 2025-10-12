using System.Text.Json.Serialization;

namespace CreditGraph.Domain.AlbumModels;

/// <summary>Page of albums returned by Spotify for an artist.</summary>
/// <summary> only includes items and next for this project, but includes the following as well:</summary>
/// <summary> href </summary>
/// <summary> limit </summary>
/// <summary> offset </summary>
/// <summary> previous </summary>
/// <summary> total </summary>
public sealed class AlbumPage
{
    [JsonPropertyName("items")]
    public List<AlbumItem>? Items { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }
}