using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<bool> SaveSingleVideoMP3(string videoUrl,bool isPlaylist=default, string outputFolder = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var youtube = new YoutubeClient();

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl,cancellationToken);
                var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();


                // You can specify both video ID or URL
                var video = await youtube.Videos.GetAsync(videoUrl,cancellationToken);
                var path = $"{video.Title}.{streamInfo.Container}";
                if (!string.IsNullOrWhiteSpace(outputFolder))
                {
                    if (isPlaylist)
                    {
                        if (!Directory.Exists(outputFolder))
                        {
                            Directory.CreateDirectory(outputFolder);
                        }
                    }

                    using (var stream = File.OpenWrite(Path.Combine(outputFolder, path)))
                    {
                        await youtube.Videos.Streams.CopyToAsync(streamInfo,
                            stream,
                            cancellationToken: cancellationToken);
                        return true;
                    }
                }
              
                // Download the stream to a file
                await youtube.Videos.Streams.DownloadAsync(streamInfo,
                    path,
                    cancellationToken:cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null,bool ignorePlaylistName=default, CancellationToken cancellationToken = default)
        {
            try
            {
                var youtube = new YoutubeClient();

                var playlist = await youtube.Playlists.GetAsync(videoUrl, cancellationToken);

                // Get all playlist videos
                var videos = await youtube.Playlists.GetVideosAsync(videoUrl,cancellationToken);
                
                foreach (var playlistVideo in videos)
                {
                    var path = outputFolder;
                    if (ignorePlaylistName)
                    {
                        path = Path.Combine(outputFolder, playlist.Title);
                    }
                    await SaveSingleVideoMP3(playlistVideo.Url,true,path, cancellationToken: cancellationToken);
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
