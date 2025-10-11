using System.Runtime.CompilerServices;
using CreditGraph.Domain;

namespace CreditGraph.Services.Interfaces;

/// <summary>
/// main services for Spotify communications
/// </summary>
public interface ISpotifyClient
{

    /// <summary>
    /// Get the User ID of the current user
    /// </summary>
    /// <param name="token"></param>
    /// <param name="ct"></param>
    /// <returns>Id of current user</returns>
    Task<string> GetCurrentUserIdAsync(
        string token,
        CancellationToken ct = default
    );

    /// <summary>
    /// Get Artist details from spotify for provided artistId
    /// </summary>
    /// <param name="artistId">The artist ID</param>
    /// <param name="token">Authentication token(base-64) from spotify</param>
    /// <param name="ct"></param>
    /// <returns>Returns Artist object with details of for given artistId</returns>
    Task<Artist> GetArtistAsync(
        string artistId,
        string token,
        CancellationToken ct = default
    );

    /// <summary>
    /// Get related Artist details from spotify for provided root artistId
    /// </summary>
    /// <param name="artistId">The root artist ID</param>
    /// <param name="token">Authentication token(base-64) from spotify</param>
    /// <param name="ct"></param>
    /// <returns>A list of related artists</returns>
    Task<List<Artist>> GetRelatedArtistAsync(
        string artistId,
        string token,
        CancellationToken ct = default
    );

    /// <summary>
    /// phase-1: Gets the albums for a provided artistId
    /// </summary>
    /// <param name="artistId"></param>
    /// <param name="token"></param>
    /// <param name="limit"></param>
    /// <param name="market"></param>
    /// <param name="includeGroups"></param>
    /// <param name="ct"></param>
    /// <returns>Returns a list of album for artistId</returns>
    Task<List<Album>> GetArtistAlbumsAsync(
        string artistId,
        string token,
        int limit,
        string market,
        IEnumerable<string> includeGroups,
        CancellationToken ct = default

    );

    /// <summary>
    /// Creates a playlist for current Spotify user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="playlistName"></param>
    /// <param name="token"></param>
    /// <param name="isPublic"></param>
    /// <param name="description"></param>
    /// <param name="ct"></param>
    /// <returns>Returns Id of created Spotify playlist</returns>
    Task<string> CreatePlaylistAsync(
        string userId,
        string playlistName,
        string token,
        bool isPublic,
        string description,
        CancellationToken ct = default
    );


    /// <summary>
    /// Adds a list of tracks to a Spotify playlist matching the provided PlaylistId
    /// </summary>
    /// <param name="playlistId"></param>
    /// <param name="trackUris"></param>
    /// <param name="token"></param>
    /// <param name="ct"></param>
    Task AddTracksToPlaylistAsync(
        string playlistId,
        List<string> trackUris,
        string token,
        CancellationToken ct = default

    );


}
