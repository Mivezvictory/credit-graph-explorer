using System.Net;
using System.Text.Json;
using CreditGraph.Services.Contracts;

namespace CreditGraph.Infrastructure.Http;

public static class HttpResponseParsing
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<ApiResult<T>> ToApiResultAsync<T>(
        this HttpResponseMessage response,
        CancellationToken ct
    )
    {
        if ((int)response.StatusCode is >= 200 and <= 299)
        {
            if (response.StatusCode == HttpStatusCode.NoContent)
                return new(true, default, response.StatusCode, null, null);

            if (response.Content is null)
                return new(true, default, response.StatusCode, null, null);
            ///
            /// if we ever handle streams. That response should be parsed here
            /// 
            var raw = await response.Content!.ReadAsStringAsync(ct).ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<T>(raw, JsonOptions);

            return new(true, result, response.StatusCode, null, null);
        }
        string? errorText = null;
        if (response.Content is not null)
            errorText = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        TimeSpan? retryAfter = null;
        if ((int)response.StatusCode == 429 &&
            response.Headers.TryGetValues("Retry-After", out var vals) &&
            int.TryParse(vals.FirstOrDefault(), out var seconds))
        {
            retryAfter = TimeSpan.FromSeconds(seconds);
        }
        return new(false, default, response.StatusCode, errorText, retryAfter);
    }
}