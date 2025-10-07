namespace CreditGraph.Domain;

public sealed record GraphEdge(
    string FromArtistId, 
    string ToArstistId,
    Relation Relation //is it wise to have a Relation of type Relation? (case)
);