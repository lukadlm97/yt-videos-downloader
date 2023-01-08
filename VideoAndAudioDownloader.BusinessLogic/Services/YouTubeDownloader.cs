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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VideoAndAudioDownloader.BusinessLogic.Models;
using VideoAndAudioDownloader.BusinessLogic.Models.DTO;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public class YouTubeDownloader:IYouTubeDownloader
    {
        private readonly ILogger<YouTubeDownloader> _logger;

        public YouTubeDownloader(ILogger<YouTubeDownloader> logger)
        {
            _logger = logger;
        }
        public async Task<bool> SaveSingleVideoMP3(string videoUrl,bool isPlaylist=default, string outputFolder =null, CancellationToken cancellationToken=default)
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

        public Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null, bool ignorePlaylistName = default,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PlaylistResponse> GetPlaylistSongs(string videoUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<SongResponse> GetSingleSong(string videoUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<DownloadResponse> DownloadAndSaveToDestinationFolder(IEnumerable<string> videoUrls, IEnumerable<string> destinationFolders,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
            UserCredential credential;
            using (var stream = new FileStream("you-tube-369316-6209172924ab.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            var channelsListRequest = youtubeService.Channels.List("contentDetails");
            channelsListRequest.Mine = true;

            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var channelsListResponse = await channelsListRequest.ExecuteAsync();

            foreach (var channel in channelsListResponse.Items)
            {
                // From the API response, extract the playlist ID that identifies the list
                // of videos uploaded to the authenticated user's channel.
                var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

                Console.WriteLine("Videos in list {0}", uploadsListId);

                var nextPageToken = "";
                while (nextPageToken != null)
                {
                    var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                    playlistItemsListRequest.PlaylistId = uploadsListId;
                    playlistItemsListRequest.MaxResults = 50;
                    playlistItemsListRequest.PageToken = nextPageToken;

                    // Retrieve the list of videos uploaded to the authenticated user's channel.
                    var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

                    foreach (var playlistItem in playlistItemsListResponse.Items)
                    {
                        // Print information about each video.
                        Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId);
                    }

                    nextPageToken = playlistItemsListResponse.NextPageToken;
                }
            }

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
