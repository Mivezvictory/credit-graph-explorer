//move to function layer before use
namespace CreditGraph.Domain;

public sealed record PlaylistCreatedRequest(
    string ArtistId,
    string Routetitle,
    int Maxtracks = 30
);