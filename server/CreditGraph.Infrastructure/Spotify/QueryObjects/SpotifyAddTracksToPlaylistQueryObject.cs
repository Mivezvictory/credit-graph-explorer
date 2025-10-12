namespace CreditGraph.Infrastructure.Spotify.QueryObjects;

public sealed record SpotifyAddTracksToPlaylistQueryObject
(
    List<string> TrackUris,//spotify album ID
    int Position = 0
);
