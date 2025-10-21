using System.Net;

namespace CreditGraph.Services.Contracts;

public sealed record ApiResult<T>(
    bool IsSuccess,
    T? Value,
    HttpStatusCode StatusCode,
    string? error,
    TimeSpan? RetryAfter);