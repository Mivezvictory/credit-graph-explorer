using CreditGraph.Services.Interfaces;
using CreditGraph.Domain;
using CreditGraph.Domain.Exceptions;
namespace CreditGraph.Services.Implentations;

/// <summary>
/// Creates a private playlist for the current user and fills it 
/// with a single reprentative track per artist(phase-1)
/// </summary>
public sealed class PlaylistService : IPlaylistService
{
    private readonly ISpotifyClient _spotify;

    public PlaylistService(ISpotifyClient spotify)
    {
        _spotify = spotify;
    }

    public async Task<string> CreateFromRouteAsync(
        ProducerTrailRoute route,
        string token,
        int maxTracks = 20,
        CancellationToken ct = default
    )
    {
        //get current user Id
        var me = await _spotify.GetCurrentUserIdAsync(token, ct);

        //create playlist - always private
        var playlistName = route.Title;
        string description = "Credit Graph Explorer - phase 1"; // should be more thoughtful name including the root artist
        var playlistId = await _spotify.CreatePlaylistAsync(me, playlistName, token, isPublic: false, description, ct);

        //select tracks for playlist
        var uris = new List<string>(Math.Min(maxTracks, route.ArtistIds.Count));
        foreach (var artistId in route.ArtistIds)
        {
            if (uris.Count >= maxTracks) break;
            var albums = await _spotify.GetArtistAlbumsAsync(
                artistId,
                token,
                limit: 1,
                "from_token",
                new[] { "album" },
                ct
            );
            var candidate = albums.FirstOrDefault()?.LeadtrackUri ?? albums.FirstOrDefault()?.Id;
            if (!string.IsNullOrEmpty(candidate))
                uris.Add(candidate);

            if (uris.Count == 0)
                throw new PlaylistCreationException("No tracks found for route.");

            await _spotify.AddTracksToPlaylistAsync(
                    playlistId,
                    uris,
                    token,
                    ct
                );
        }

        return playlistId;

    }
}