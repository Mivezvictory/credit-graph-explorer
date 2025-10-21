using CreditGraph.Services.Interfaces;
using CreditGraph.Domain;
namespace CreditGraph.Services.Implentations;

/// <summary>
/// Builds a artist credit graph using selected and related artist
/// phase-1: no caching
/// </summary>
public class GraphBuilder : IGraphBuilder
{
    private readonly ISpotifyClient _spotify;
    private readonly ILastfmClient _lastfm;
    private readonly IMusicBrainzClient _mb;

    public GraphBuilder(
        ISpotifyClient spotyfy,
        ILastfmClient lastfm,
        IMusicBrainzClient mb
    )
    {
        _spotify = spotyfy;
        _lastfm = lastfm;
        _mb = mb;
    }

    public async Task<ArtistGraph> BuildAsync(
        string artistId,
        string token,
        CancellationToken ct = default)
    {
        var root = await _spotify.GetArtistAsync(artistId, token, ct);
        
        var related = await _lastfm.GetRelatedArtistAsync(root.Name, ct);

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