namespace CreditGraph.Domain.Exceptions
{
    public class SpotifyCallbackException : Exception
    {
        public SpotifyCallbackException() { }
        public SpotifyCallbackException(string message) : base(message) { }
        public SpotifyCallbackException(string message, System.Exception inner) : base(message, inner) { }
    }
}
