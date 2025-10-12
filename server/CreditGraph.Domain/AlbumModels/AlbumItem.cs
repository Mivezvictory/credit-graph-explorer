using System.Text.Json.Serialization;

namespace CreditGraph.Domain.AlbumModels;

/// <summary>Minimal album fields (id, name, release_date, release_date_precision, album_group, album_type).</summary>
public sealed class AlbumItem
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("release_date")] public string? ReleaseDate { get; set; }
    [JsonPropertyName("release_date_precision")] public string? ReleaseDatePrecision { get; set; }
    [JsonPropertyName("album_group")] public string? AlbumGroup { get; set; }
    [JsonPropertyName("album_type")] public string? AlbumType { get; set; }
}