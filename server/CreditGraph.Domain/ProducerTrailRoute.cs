namespace CreditGraph.Domain;

public sealed record ProducerTrailRoute(
    string Title,
    IReadOnlyList<string> ArtistIds
);