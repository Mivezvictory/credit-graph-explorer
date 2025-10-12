using CreditGraph.Domain;
namespace CreditGraph.Services.Interfaces;

public interface ILastfmClient
{
    /// <summary>
    /// Get a list of related Artist from Last.fm API for provided root artistId
    /// </summary>
    /// <param name="artistId">The root artist ID</param>
    /// <param name="token">Authentication token(base-64) from spotify</param>
    /// <param name="ct"></param>
    /// <returns>A list of related artists</returns>
    Task<List<Artist>> GetRelatedArtistAsync(
        string artistName,
        CancellationToken ct = default
    );

    
}