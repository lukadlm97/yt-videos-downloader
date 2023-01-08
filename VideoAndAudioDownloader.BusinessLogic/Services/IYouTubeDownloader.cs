using VideoAndAudioDownloader.BusinessLogic.Enumerations;
using VideoAndAudioDownloader.BusinessLogic.Models.DTO;

namespace VideoAndAudioDownloader.BusinessLogic.Services;

public interface IYouTubeDownloader
{
    Task<bool> SaveSingleVideoMP3(string videoUrl, bool isPlaylist = default, string outputFolder = null,
        CancellationToken cancellationToken = default);

    Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null, bool ignorePlaylistName = default,
        CancellationToken cancellationToken = default);

    Task<PlaylistResponse> GetPlaylistSongs(string videoUrl, CancellationToken cancellationToken = default);
    Task<SongResponse> GetSingleSong(string videoUrl, CancellationToken cancellationToken = default);

    Task<DownloadResponse> DownloadAndSaveToDestinationFolder(IEnumerable<string> videoUrls,
        IEnumerable<string> destinationFolders,Enumerations.MediaType mediaType=MediaType.None, CancellationToken cancellationToken = default);
}