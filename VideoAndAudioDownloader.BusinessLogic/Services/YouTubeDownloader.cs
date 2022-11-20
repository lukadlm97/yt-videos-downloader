using MediaToolkit.Model;
using MediaToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;
using static MediaToolkit.Model.Metadata;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public class YouTubeDownloader:IYouTubeDownloader
    {
        private readonly ILogger<YouTubeDownloader> _logger;

        public YouTubeDownloader(ILogger<YouTubeDownloader> logger)
        {
            _logger = logger;
        }
        public async Task<bool> SaveSingleVideoMP3(string videoUrl, string outputFolder =null, CancellationToken cancellationToken=default)
        {
            try
            {
                var youYube = YouTube.Default;
                var video = await youYube.GetVideoAsync(videoUrl);

                return await ProcessSingleVideo(video, outputFolder, cancellationToken); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return
                    false;
            }
         
        }

        public async Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var youYube = YouTube.Default;
                bool flag = true;
                var videoUrls = await GetUrlsFromPlaylist(videoUrl);
                foreach (var youTubeVideoUrl in videoUrls)
                {
                    var youTubeVideo = await youYube.GetVideoAsync(youTubeVideoUrl);
                    flag &=await ProcessSingleVideo(youTubeVideo,outputFolder, cancellationToken);

                }

                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return
                    false;
            }
        }

        private  async Task<IEnumerable<string>> GetUrlsFromPlaylist(string playlistUrl)
        {
            string url = $"list={playlistUrl}";
            HttpClient client = new HttpClient();
            _logger.LogInformation(url);
            client.BaseAddress = new Uri($"https://www.youtube.com/playlist?");
            var response  = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(response.Content.ToString());
                var playlistData = JsonConvert.DeserializeObject<IEnumerable<string>>(response.Content.ToString());
                return playlistData;
            }
            _logger.LogInformation(response.StatusCode+response.Content.ToString());
            return null;

        }

        async Task<bool> ProcessSingleVideo(YouTubeVideo youTubeVideo,string outputFolder=null,CancellationToken cancellationToken=default)
        {
            try
            {
                var videoFilePath = outputFolder != null ? outputFolder + youTubeVideo.FullName : youTubeVideo.FullName;
                await File.WriteAllBytesAsync(videoFilePath, await youTubeVideo.GetBytesAsync(),
                    cancellationToken);
                var inputFile = new MediaFile { Filename = videoFilePath };
                var outputFile = new MediaFile { Filename = $"{youTubeVideo.FullName}.mp3" };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);

                    engine.Convert(inputFile, outputFile);
                }

                if (File.Exists(Path.Combine(videoFilePath)))
                {
                    File.Delete(Path.Combine(videoFilePath));
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return
                    false;
            }
          
        }
    }
}
