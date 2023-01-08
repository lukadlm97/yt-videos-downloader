using VideoAndAudioDownloader.BusinessLogic.Enumerations;
using VideoAndAudioDownloader.BusinessLogic.Models;
using VideoAndAudioDownloader.BusinessLogic.Models.DTO;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace VideoAndAudioDownloader.BusinessLogic.Services;

public class ExplodeYouTubeDownloader : IYouTubeDownloader
{
    public async Task<bool> SaveSingleVideoMP3(string videoUrl, bool isPlaylist = default, string outputFolder = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var youtube = YouTubeClientFactory.CreateYoutubeClient();

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl, cancellationToken);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();


            // You can specify both video ID or URL
            var video = await youtube.Videos.GetAsync(videoUrl, cancellationToken);
            var path = $"{video.Title}.{streamInfo.Container}";
            if (!string.IsNullOrWhiteSpace(outputFolder))
            {
                if (isPlaylist)
                    if (!Directory.Exists(outputFolder))
                        Directory.CreateDirectory(outputFolder);

                await using var stream = File.OpenWrite(Path.Combine(outputFolder, path));
                await youtube.Videos.Streams.CopyToAsync(streamInfo,
                    stream,
                    cancellationToken: cancellationToken);
                return true;
            }

            // Download the stream to a file
            await youtube.Videos.Streams.DownloadAsync(streamInfo,
                path,
                cancellationToken: cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null,
        bool ignorePlaylistName = default, CancellationToken cancellationToken = default)
    {
        try
        {
            var youtube = YouTubeClientFactory.CreateYoutubeClient();

            var playlist = await youtube.Playlists.GetAsync(videoUrl, cancellationToken);

            // Get all playlist videos
            var videos = await youtube.Playlists.GetVideosAsync(videoUrl, cancellationToken);

            foreach (var playlistVideo in videos)
            {
                var path = outputFolder;
                if (ignorePlaylistName) path = Path.Combine(outputFolder, playlist.Title);
                await SaveSingleVideoMP3(playlistVideo.Url, true, path, cancellationToken);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<PlaylistResponse> GetPlaylistSongs(string videoUrl, CancellationToken cancellationToken = default)
    {
        var response = new PlaylistResponse
        {
            OperationStatus = OperationStatus.Success
        };
        try
        {
            var youtube = YouTubeClientFactory.CreateYoutubeClient();

            var playlist = await youtube.Playlists.GetAsync(videoUrl, cancellationToken);
            if (playlist == null)
            {
                response.OperationStatus = OperationStatus.NotFound;
                return response;
            }

            var realPlaylist = new Playlist
            {
                Name = playlist.Title
            };
            var realSongs = new List<Song>();
            // Get all playlist videos
            var videos = await youtube.Playlists.GetVideosAsync(videoUrl, cancellationToken);

            foreach (var playlistVideo in videos)
            {
                var song = new Song
                {
                    Title = playlistVideo.Title,
                    Channel = playlistVideo.Author.ChannelTitle,
                    Duration = playlistVideo.Duration.ToString(),
                    Url = playlistVideo.Url
                };
                realSongs.Add(song);
            }

            realPlaylist.Songs = realSongs;
            response.Playlist = realPlaylist;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            response.OperationStatus = OperationStatus.BadRequest;
        }

        return response;
    }

    public async Task<SongResponse> GetSingleSong(string videoUrl, CancellationToken cancellationToken = default)
    {
        var response = new SongResponse
        {
            OperationStatus = OperationStatus.Success
        };
        try
        {
            var youtube = YouTubeClientFactory.CreateYoutubeClient();

            var video = await youtube.Videos.GetAsync(videoUrl, cancellationToken);
            if (video is null)
            {
                response.OperationStatus = OperationStatus.NotFound;
                return response;
            }

            var song = new Song
            {
                Title = video.Title,
                Channel = video.Author.ChannelTitle,
                Duration = video.Duration.ToString(),
                Url = video.Url
            };

            response.Song = song;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            response.OperationStatus = OperationStatus.BadRequest;
        }

        return response;
    }

    public async Task<DownloadResponse> DownloadAndSaveToDestinationFolder(IEnumerable<string> videoUrls,
        IEnumerable<string> destinationFolders,
        MediaType mediaType=MediaType.Audio,
        CancellationToken cancellationToken = default)
    {
        var response = new DownloadResponse
        {
            OperationStatus = OperationStatus.Success
        };
        try
        {
            if (!videoUrls.Any() || !destinationFolders.Any())
            {
                response.OperationStatus = OperationStatus.MissingParts;
                return response;
            }

            foreach (var videoUrl in videoUrls)
                if (!await SaveSingleVideoMP3(videoUrl, destinationFolders, mediaType, cancellationToken))
                    response.OperationStatus = OperationStatus.MissingParts;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            response.OperationStatus = OperationStatus.BadRequest;
        }

        return response;
    }

    public async Task<bool> SaveSingleVideoMP3(string videoUrl
        , IEnumerable<string> outputFolders,Enumerations.MediaType mediaType=MediaType.None, CancellationToken cancellationToken = default)
    {
        try
        {
            var youtube = YouTubeClientFactory.CreateYoutubeClient();

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl, cancellationToken);
            IStreamInfo? streamInfo = null;
            if (mediaType == Enumerations.MediaType.None)
            {
                return false;
            }
            
            if (mediaType == Enumerations.MediaType.Video)
            {
                streamInfo= streamManifest.GetMuxedStreams().GetWithHighestBitrate();
            }

            if (mediaType == MediaType.Audio)
            {
                streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            }

            if (streamInfo == null)
            {
                return false;
            }
            // You can specify both video ID or URL
            var video = await youtube.Videos.GetAsync(videoUrl, cancellationToken);
            var path = $"{video.Title}.{streamInfo.Container}";
            
            var outputFolder = outputFolders.First();
            if (!string.IsNullOrWhiteSpace(outputFolder))
            {
                if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

                var originalFile = Path.Combine(outputFolder, path);
                await using var stream = File.OpenWrite(originalFile);
                await youtube.Videos.Streams.CopyToAsync(streamInfo,
                    stream,
                    cancellationToken: cancellationToken);

                await stream.DisposeAsync();
                foreach (var folder in outputFolders.Skip(1))
                {
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    var copyPathFile = Path.Combine(folder, path);
                    File.Copy(originalFile, copyPathFile);
                }
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