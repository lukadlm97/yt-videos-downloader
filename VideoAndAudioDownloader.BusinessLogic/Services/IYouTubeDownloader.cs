using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public interface IYouTubeDownloader
    {
        Task<bool> SaveSingleVideoMP3(string videoUrl,bool isPlaylist=default, string outputFolder = null, CancellationToken cancellationToken = default);
        Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null,bool ignorePlaylistName=default, CancellationToken cancellationToken = default);
        Task<Models.DTO.PlaylistResponse> GetPlaylistSongs(string videoUrl,CancellationToken cancellationToken = default);
        Task<Models.DTO.SongResponse> GetSingleSong(string videoUrl,CancellationToken cancellationToken = default);

        Task<Models.DTO.DownloadResponse> DownloadAndSaveToDestinationFolder(IEnumerable<string> videoUrls,
            IEnumerable<string> destinationFolders, CancellationToken cancellationToken = default);
    }
}
