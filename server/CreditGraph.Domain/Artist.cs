namespace CreditGraph.Domain;

public sealed record Artist(
    string Id,//spotify artist ID
    string Name,
    string? SpotifyUrl = null,
    string? MusicBrainzId = null
);