using System.Net;
using Microsoft.AspNetCore.Mvc;
namespace CreditGraph.Functions.Functions.Common;

public static class HttpJson
{
    public static ActionResult CreateReturnResponse(string returnObj, int statusCode)
    {
        return new ContentResult
        {
            Content = returnObj,
            ContentType = "application/json",
            StatusCode = GetStatusCode(statusCode)
        };
    }

    private static int GetStatusCode(int statusCode)
    {
        return statusCode switch
        {
            //add as needed
            200 => (int)HttpStatusCode.OK,
            302 => (int)HttpStatusCode.Redirect,
            400 => (int)HttpStatusCode.BadRequest,
            401 => (int)HttpStatusCode.Unauthorized,
            404 => (int)HttpStatusCode.NotFound,
            412 => (int)HttpStatusCode.PreconditionFailed,
            429 => (int)HttpStatusCode.TooManyRequests,
            _ => (int)HttpStatusCode.BadRequest

        };
    }
}