using CreditGraph.Services.Interfaces;

namespace CreditGraph.Infrastructure.MusicBrainz;

/// <summary>
/// Default implementation of <see cref="IMusicBrainzClient"/> backed by a typed HttpClient.
/// </summary>
public class MusicBrainzClient : IMusicBrainzClient
{
    private readonly HttpClient _http;
    public MusicBrainzClient(HttpClient http)
    {
        _http = http;
    }
}