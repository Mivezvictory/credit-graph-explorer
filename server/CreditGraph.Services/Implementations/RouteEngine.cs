using CreditGraph.Services.Interfaces;
using CreditGraph.Domain;
using System.ComponentModel;

namespace CreditGraph.Services.Implentations;

/// <summary>
/// phase-1 route: A simple path starting at the root artist
/// followed by all directly-related artists sorted by Name(then Id as a tie -break)
/// </summary>
public sealed class RouteEngine : IRouteEngine
{
    public Task<IReadOnlyList<ProducerTrailRoute>> GetRoutesAsync(
        ArtistGraph graph,
        CancellationToken ct = default
    )
    {
        if (graph.Artists.Count == 0)
            return Task.FromResult<IReadOnlyList<ProducerTrailRoute>>(Array.Empty<ProducerTrailRoute>());

        //pick directly-related neighbors of the root
        var direct = graph
            .Edges
            .Where(e => e.FromArtistId == graph.RootArtistId && e.Relation == Relation.Related)
            .Select(e => e.ToArstistId)
            .Distinct()
            .ToList();

        direct.Sort((aId, bId) =>
        {
            var a = graph.Artists[aId];
            var b = graph.Artists[bId];
            var byName = string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
            return byName != 0 ? byName : string.Compare(a.Id, b.Id, StringComparison.Ordinal);
        });//need to understand or re-write
           //direct graph is sorted aphabetically then by ID for a tie break 

        var ordered = new List<string>(1 + direct.Count) { graph.RootArtistId };
        ordered.AddRange(direct);

        var route = new ProducerTrailRoute("Producer Trail (beta)", ordered);

        return Task.FromResult<IReadOnlyList<ProducerTrailRoute>>(new[] { route });

    }
}