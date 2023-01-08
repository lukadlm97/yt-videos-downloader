using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoAndAudioDownloader.BusinessLogic.Enumerations;
using VideoAndAudioDownloader.BusinessLogic.Models.DTO;

namespace VideoAndAudioDownloader.Desktop.Models
{
    public interface IYouTubeService
    {
        Task<PlaylistResponse> GetPlaylist(string videoUrl);
        Task<SongResponse> GetSong(string videoUrl);
        Task<DownloadResponse> DownloadAndSave(IEnumerable<string> videoUrls,
            IEnumerable<string> destinationFolders,
            MediaType mediaType=MediaType.Audio);

    }
}
