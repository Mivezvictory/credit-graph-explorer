using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CreditGraph.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CreditGraph.Domain.Exceptions;
using CreditGraph.Functions.Handlers;
using CreditGraph.Domain.Models.Spotify;

namespace CreditGraph.Functions.Functions;
/// <summary>
/// GET /api/spotify-callback
/// Handles Spotify's redirect, exchanges authorization code for tokens,
/// then redirects the user back to the client (SPA).
/// </summary>
public class SpotifyCallback
{
    private readonly ISpotifyCallbackHandler _handler;
    private readonly SpotifyOptions _spotify;
    private readonly ClientAppOptions _client;
    private ILogger<SpotifyCallback> _logger;
    private static readonly HttpClient _http = new HttpClient();

    public SpotifyCallback(
        ISpotifyCallbackHandler handler,
        IOptions<SpotifyOptions> spotify,
        IOptions<ClientAppOptions> client,
        ILogger<SpotifyCallback> logger)
    {
        _handler = handler;
        _spotify = spotify.Value;
        _client = client.Value;
        _logger = logger;

    }//constructor

    [Function("SpotifyCallback")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "spotify-callback")] HttpRequest req)
    {
        try
        {
            var code = _handler.GetAuthCode(req);
            var state = req.Query["state"];
            SpotifyTokenResponse? tokens = await _handler.ExchangeCodeForTokensAsync(code, _spotify);
            if (tokens == null || string.IsNullOrEmpty(tokens.AccessToken))
            {
                return CreateBadResponse("Failed to exchange authorization code.", "Token_Exchange_Failed");
            }
            if (string.IsNullOrWhiteSpace(state))
                state = "/graph";
            var dest = _handler.CombineUrl(_client.BaseUrl, state!);
            var redirectTo = $"{dest}{(dest.Contains("?") ? "&" : "?")}tokens={Uri.EscapeDataString(tokens.AccessToken)}";
            return new RedirectResult(redirectTo);
        }
        catch (SpotifyCallbackException ex)
        {
            return CreateBadResponse(ex.Message, "Invalid_callback");
        }
        catch (Exception ex)
        {
            return CreateBadResponse(ex.Message, "Token_Exchange_Failed");
        }


    }

    private IActionResult CreateBadResponse(string message, string errorTitle)
    {
        _logger.LogError(message);
        var errorDetails = new
        {
            error = "Token_Exchange",
            message = message
        };
        return new BadRequestObjectResult(errorDetails);
    }
}
