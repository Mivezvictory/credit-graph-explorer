namespace CreditGraph.Services.Interfaces;

/// <summary>
/// Minimal client for MusicBrainz metadata (artist resolve, releases with credits).
/// Abstracting MB lets us throttle, add retries, and test without hitting the network.
/// </summary>
public interface IMusicBrainzClient { /* methods added later */ }
