using System.Runtime.CompilerServices;
using CreditGraph.Domain;

namespace CreditGraph.Services.Interfaces;

/// <summary>
/// main services for Spotify communications
/// </summary>
public interface ISpotifyClient
{
    /// <summary>
    /// Get Artist details from spotify for provided artistId
    /// </summary>
    /// <param name="artistId">The artist ID</param>
    /// <param name="token">Authentication token(base-64) from spotify</param>
    /// <param name="ct"></param>
    /// <returns></returns>
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
    /// <returns></returns>
    Task<List<Artist>> GetRelatedArtistAsync(
        string artistId,
        string token,
        CancellationToken ct = default
    );


}
