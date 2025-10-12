namespace CreditGraph.Infrastructure.Spotify.QueryObjects;

public sealed record SpotifyPlaylistQueryObj
(
    string Name,//spotify album ID
    string Description,
    bool Public = false,
    bool Collaborative = false
);
