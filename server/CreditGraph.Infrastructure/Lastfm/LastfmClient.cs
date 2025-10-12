using CreditGraph.Services.Interfaces;
using CreditGraph.Domain;
namespace CreditGraph.Infrastructure.Lastfm;

public class LastfmClient : ILastfmClient
{
    private readonly HttpClient _http;
    public LastfmClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Artist>> GetRelatedArtistAsync(
        string artistName,
        CancellationToken ct = default
    )
    {
        //hard coding so testing purposes, will implement to use last.fm API in a different feature branch
        List<Artist> artists = new List<Artist>
        {
            new Artist("4CvTDPKA6W06DRfBnZKrau?si=wZDYXxX7TvKOO3y57BQcgw", "Thom Yorke"),
            new Artist("7tA9Eeeb68kkiG9Nrvuzmi?si=mh6l4ViNR32MMsqa7d7SOQ", "Atoms for Peace"),
            new Artist("3nnQpaTvKb5jCQabZefACI?si=adn_eMqfR2-zjL8mRPZd6g", "Jeff Buckley"),
            new Artist("0epOFNiUfyON9EYx7Tpr6V?si=5B3GtZ6tQU-MPy1CqrJ9Ug", "The Strokes"),
            new Artist("12Chz98pHFMPJEknJQMWvI?si=WfAG5FhdT6aCcKpJo9UHSg", "Muse"),

        };
        return await Task.FromResult<List<Artist>>(artists);
    }

}