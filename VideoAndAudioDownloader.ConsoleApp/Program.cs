// See https://aka.ms/new-console-template for more information

using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VideoAndAudioDownloader.BusinessLogic.Scripts;
using VideoAndAudioDownloader.BusinessLogic.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext,services) =>
    {
        services.Configure<VideoAndAudioDownloader.BusinessLogic.Configurations.HostedServiceSettings>(
            hostingContext.Configuration.GetSection(nameof(VideoAndAudioDownloader.BusinessLogic.Configurations
                .HostedServiceSettings)));

        services.AddSingleton<IYouTubeDownloader, YouTubeDownloader>();
    });




var app = ConsoleApp.CreateFromHostBuilder(host, args);
app.AddCommands<Script>();
await app.RunAsync();
