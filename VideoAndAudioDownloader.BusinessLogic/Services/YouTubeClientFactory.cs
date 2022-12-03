using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public class YouTubeClientFactory
    {
        public static YoutubeClient CreateYoutubeClient()
        {
            return new YoutubeClient();
        }
    }
}
