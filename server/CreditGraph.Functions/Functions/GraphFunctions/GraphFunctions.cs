using CreditGraph.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using CreditGraph.Functions.Functions.Common;
using System.Text.Json;
using CreditGraph.Domain.Exceptions;
using CreditGraph.Services.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;


namespace CreditGraph.Functions.Functions.GraphFunctions;

public class GraphFunctions
{
    private readonly IGraphBuilder _builder;
    private readonly ILogger<GraphFunctions> _logger;

    public GraphFunctions(
        IGraphBuilder builder,
        ILogger<GraphFunctions> logger
    )
    {
        _builder = builder;
        _logger = logger;
    }

    [Function("GetGraph")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "graph")] HttpRequest req)
    {
        var artistId = req.Query["artistId"];
        var token = req.Query["token"];

        if (string.IsNullOrWhiteSpace(artistId) || string.IsNullOrWhiteSpace(token))
            return new BadRequestObjectResult(new { error = "artistId and token are required" });
        try
        {
            var graph = await _builder.BuildAsync(artistId!, token!);
            return HttpJson.CreateReturnResponse(JsonSerializer.Serialize(graph), 200);
        }
        catch (SpotifyException ex)
        {

            return ex switch
            {
                //align error messages
                SpotifyArtistNotFoundException => HttpJson.CreateReturnResponse(ex.Message, 404),
                SpotifyUnauthorizedException => HttpJson.CreateReturnResponse(ex.Message, 404),
                SpotifyRateLimitedException r => HttpJson.CreateReturnResponse(ex.Message, 429),
                SpotifyProfileInvalidException => HttpJson.CreateReturnResponse(ex.Message, 502),
                SpotifyUnavailableException => HttpJson.CreateReturnResponse(ex.Message, 503),
            };
        }
    }
}
//error messages need to be aligned into a single errorshape for entire project