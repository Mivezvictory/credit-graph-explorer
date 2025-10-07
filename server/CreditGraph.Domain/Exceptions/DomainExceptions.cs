namespace CreditGraph.Domain.Exceptions;

public class DomainExcetion : Exception
{
    public DomainExcetion(string message) : base(message) { }
}

public sealed class ArtistNotFoundException : DomainExcetion
{
    public ArtistNotFoundException(string artistId)
    : base($"Artist not found: {artistId}") { }
}

public sealed class GraphNotBuiltException : DomainExcetion
{
    public GraphNotBuiltException(string artistId)
    : base($"Graph has not been built yet for artist: {artistId}") { }
}

public sealed class PlaylistCreationException : DomainExcetion
{
    public PlaylistCreationException(string reason)
    : base($"Playlist creation failed: {reason}") { }
}