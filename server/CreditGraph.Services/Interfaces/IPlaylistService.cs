using CreditGraph.Domain;

namespace CreditGraph.Services.Interfaces;

/// <summary>
/// Turns a selected route int a real spotify playlist for the current user
/// phase-1 
/// </summary>
public interface IPlaylistService
{
    Task<string> CreateFromRouteAsync(
        ProducerTrailRoute route,
        string token,
        int maxTracks = 20,
        CancellationToken ct = default
    );
}