using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public class ExplodeYouTubeDownloader:IYouTubeDownloader
    {
        public async Task<bool> SaveSingleVideoMP3(string videoUrl, string outputFolder = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var youtube = new YoutubeClient();

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl,cancellationToken);
                var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();


                // You can specify both video ID or URL
                var video = await youtube.Videos.GetAsync(videoUrl,cancellationToken);
           
                // Download the stream to a file
                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{video.Title}.{streamInfo.Container}",cancellationToken:cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var youtube = new YoutubeClient();

                var playlist = await youtube.Playlists.GetAsync(videoUrl, cancellationToken);


                // Get all playlist videos
                var videos = await youtube.Playlists.GetVideosAsync(videoUrl,cancellationToken);

                foreach (var playlistVideo in videos)
                {
                    await SaveSingleVideoMP3(playlistVideo.Url, cancellationToken: cancellationToken);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }
    }
}
