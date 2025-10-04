using CreditGraph.Options;
using CreditGraph.Services.Implentations;
using CreditGraph.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddOptions<SpotifyOptions>().BindConfiguration("Spotify");
builder.Services.AddOptions<ClientAppOptions>().BindConfiguration("ClientApp");
builder.Services.AddOptions<MusicBrainzOptions>().BindConfiguration("MusicBrainz");

builder.Services.AddHttpClient<ISpotifyClient, SpotifyClient>(client =>
{
    client.BaseAddress = new Uri("https://api.spotify.com/");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
});

builder.Services.AddHttpClient<IMusicBrainzClient, MusicBrainzClient>((sp, client) =>
{
    var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MusicBrainzOptions>>().Value;
    client.BaseAddress = new Uri(opts.BaseUrl);
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    client.DefaultRequestHeaders.UserAgent.ParseAdd(opts.UserAgent); // MB required
});



builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
