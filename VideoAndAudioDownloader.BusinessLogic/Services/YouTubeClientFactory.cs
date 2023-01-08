using YoutubeExplode;

namespace VideoAndAudioDownloader.BusinessLogic.Services;

public class YouTubeClientFactory
{
    public static YoutubeClient CreateYoutubeClient()
    {
        return new YoutubeClient();
    }
}