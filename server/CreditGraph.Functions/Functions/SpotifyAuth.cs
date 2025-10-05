using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CreditGraph.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace CreditGraph.Functions.Functions;

/// <summary>
/// GET /api/spotify-auth
/// Builds the Spotify /authorize URL and 302-redirects the browser to it.
/// This starts the OAuth flow.
/// </summary>
public class SpotifyAuth
{
    private SpotifyOptions _spotify;
    private readonly ILogger<SpotifyAuth> _logger;
    private const string Scopes = "playlist-modify-private playlist-modify-public";


    public SpotifyAuth(IOptions<SpotifyOptions> spotify, ILogger<SpotifyAuth> logger)
    {
        _spotify = spotify.Value;
        _logger = logger;
    }

    [Function("SpotifyAuth")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "spotify-auth")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger(spotify-auth) processed a request.");

        try
        {
            var state = req.Query["state"];
            state = string.IsNullOrEmpty(state) ? "" : Uri.EscapeDataString(state!);
            //state = Uri.EscapeDataString(req.Query["state"]);
            var rawRedirect = _spotify?.RedirectUri;
            var clientId = _spotify?.ClientId;

            // Validate raw values before URL-encoding so we can detect missing configuration
            if (string.IsNullOrEmpty(rawRedirect))
            {
                _logger.LogError("Spotify RedirectUri is missing or not configured in app settings. RawRedirect='{RawRedirect}' ClientId='{ClientId}'", rawRedirect, clientId);
                return new BadRequestObjectResult("Spotify redirect URI is not configured. Check local.settings.json or environment variables (Spotify__RedirectUri).");
            }

            var redirectUri = Uri.EscapeDataString(rawRedirect);

            var url =
                $"https://accounts.spotify.com/authorize?response_type=code" +
                $"&client_id={clientId}" +
                $"&redirect_uri={redirectUri}" +
                $"&scope={HttpUtility.UrlEncode(Scopes)}" +
                (string.IsNullOrEmpty(state) ? "" : $"&state={state}");

            _logger.LogInformation("Redirecting to Spotify authorize. RedirectUri={RedirectUri}", rawRedirect);
            //var response = req.CreateResponse(HttpStatusCode.Redirect);
            return new RedirectResult(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }

    }
}
