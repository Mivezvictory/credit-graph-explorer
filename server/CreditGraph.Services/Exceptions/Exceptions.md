## Exception Mapping

- 401/403 → ```throw new SpotifyUnauthorizedException();```
- 429 → ```throw new SpotifyRateLimitedException(retryAfter);```
- 5xx/timeout → ```throw new SpotifyUnavailableException("Spotify API unavailable", ex);```
- /v1/me missing id → ```throw new SpotifyProfileInvalidException("Missing 'id' in /v1/me");```