using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CreditGraph.Functions.Functions;

/// <summary>
/// Simple function to test if Host is running
/// </summary>
public class Health
{
    /// <summary>
    /// GET /api/health → 200 {"status":"ok"}
    /// </summary>
    [Function("Health")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteAsJsonAsync(new { status = "ok" });
        return res;
    }
}
