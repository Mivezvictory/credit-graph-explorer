using CreditGraph.Domain;

namespace CreditGraph.Services.Interfaces;

/// <summary>
/// Produces ordered routes from a prebuilt ArtistGraph
/// Phase-1 returns a single deterministic "Producer trail (beta)" route
/// </summary>
public interface IRouteEngine
{
    Task<IReadOnlyList<ProducerTrailRoute>> GetRoutesAsync(
        ArtistGraph graph,
        CancellationToken ct = default
    );
}