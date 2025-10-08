using CreditGraph.Services.Interfaces;
using CreditGraph.Domain;
using CreditGraph.Domain.Exceptions;

namespace CreditGraph.Services.Implentations;

/// <summary>
/// Builds a artist credit graph using selected and related artist
/// phase-1: no caching
/// </summary>
public class GraphBuilder : IGraphBuilder
{
    private ISpotifyClient _spotify;
    private IMusicBrainzClient _mb;

    public GraphBuilder(
        ISpotifyClient spotyfy,
        IMusicBrainzClient mb
    )
    {
        _spotify = spotyfy;
        _mb = mb;
    }

    public async Task<ArtistGraph> BuildAsync(
        string artistId,
        string token,
        CancellationToken ct = default)
    {
        var root = await _spotify.GetArtistAsync(artistId, token, ct);
        if (root is null)
            throw new ArtistNotFoundException(artistId);

        var related = await _spotify.GetRelatedArtistAsync(artistId, token, ct);

        var artists = new Dictionary<string, Artist>(StringComparer.Ordinal);
        artists[root.Id] = root;
        foreach (var a in related)
            artists[a.Id] = a;


        var edges = new List<GraphEdge>(related.Count);
        foreach (var a in related)
            edges.Add(new GraphEdge(root.Id, a.Id, Relation.Related));

        return new ArtistGraph(root.Id, artists, edges);

    }

}