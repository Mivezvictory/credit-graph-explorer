namespace CreditGraph.Domain;

public sealed record ArtistGraph(
    string RootArtistId,
    IReadOnlyDictionary<string, Artist> Artists,
    IReadOnlyList<GraphEdge> Edges
);