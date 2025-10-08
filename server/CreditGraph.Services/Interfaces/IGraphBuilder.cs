using CreditGraph.Domain;

namespace CreditGraph.Services.Interfaces;

/// <summary>
/// Builds an artist-centered graph from first-party data
/// ***Future enriment***
/// </summary>
public interface IGraphBuilder
{
    /// <summary>
    /// Build a graph for the given Spotify artist id 
    /// Throws ArtistNotFoundException if the root artist cannot be found
    /// </summary>
    /// <param name="artistId"></param>
    /// <param name="token"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ArtistGraph> BuildAsync(
        string artistId,
        string token,
        CancellationToken ct = default);
}