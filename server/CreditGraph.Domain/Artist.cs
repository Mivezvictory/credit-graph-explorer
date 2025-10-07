namespace CreditGraph.Domain;

public sealed record Artist(
    string Id,//spotify artust ID
    string Name,
    String? SpotifyUrl = null,
    string? MusicBrainzId = null
);