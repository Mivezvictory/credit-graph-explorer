using CreditGraph.Services.Interfaces;
namespace CreditGraph.Services.Implentations;

/// <summary>
/// Default implementation of <see cref="IMusicBrainzClient"/> using a typed HttpClient.
/// The HttpClient includes required headers (User-Agent) and a base URL,
/// set in Program.cs. Any polite throttling/retry policies can also be added centrally.
/// </summary>
public class MusicBrainzClient : IMusicBrainzClient
{
    private readonly HttpClient _http;

    /// <summary>
    /// The typed HttpClient is configured in Program.cs (base address, Accept, User-Agent).
    /// </summary>
    public MusicBrainzClient(HttpClient http) => _http = http;
}
