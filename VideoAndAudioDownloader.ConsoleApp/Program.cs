// See https://aka.ms/new-console-template for more information

using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using VideoAndAudioDownloader.BusinessLogic.Scripts;
using VideoAndAudioDownloader.BusinessLogic.Services;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext,services) =>
    {
        services.Configure<VideoAndAudioDownloader.BusinessLogic.Configurations.HostedServiceSettings>(
            hostingContext.Configuration.GetSection(nameof(VideoAndAudioDownloader.BusinessLogic.Configurations
                .HostedServiceSettings)));

        services.AddSingleton<IYouTubeDownloader, ExplodeYouTubeDownloader>();
    });




var app = ConsoleApp.CreateFromHostBuilder(host, args);
app.AddCommands<Script>();
await app.RunAsync();

/*
IYouTubeDownloader youTubeDownloader = new ExplodeYouTubeDownloader();
await youTubeDownloader.SavePlaylistMP3("https://www.youtube.com/playlist?list=PL0-G9moQHFshAHypJ7NA55dsU7viJq7XZ");

*/


