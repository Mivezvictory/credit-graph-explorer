namespace CreditGraph.Services.Exceptions;

public abstract class SpotifyException : Exception
{
    protected SpotifyException(string message, Exception? inner = null) : base(message, inner) { }
}

public sealed class SpotifyUnauthorizedException : SpotifyException
{
    public SpotifyUnauthorizedException(string message = "Unauthorized") : base(message) { }
}

public sealed class SpotifyRateLimitedException : SpotifyException
{
    public TimeSpan? RetryAfter { get; }
    public SpotifyRateLimitedException(TimeSpan? retryAfter = null, string message = "Rate limited")
        : base(message) => RetryAfter = retryAfter;
}

public sealed class SpotifyProfileInvalidException : SpotifyException
{
    public SpotifyProfileInvalidException(string message) : base(message) { }
}

public sealed class SpotifyUnavailableException : SpotifyException
{
    public SpotifyUnavailableException(string message, Exception? inner = null) : base(message, inner) { }
}

public sealed class SpotifyPlaylistException : SpotifyException
{
    public SpotifyPlaylistException(string message, Exception? inner = null) : base(message, inner) { }
}
