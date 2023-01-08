using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoAndAudioDownloader.BusinessLogic.Enumerations;
using VideoAndAudioDownloader.BusinessLogic.Models.DTO;
using VideoAndAudioDownloader.BusinessLogic.Services;

namespace VideoAndAudioDownloader.Desktop.Models
{
    public class YouTubeService:IYouTubeService
    {
        public YouTubeService(IYouTubeDownloader youTubeDownloader)
        {
            this.youTubeDownloader = youTubeDownloader;
        }
        private readonly IYouTubeDownloader youTubeDownloader;

  

        public async Task<PlaylistResponse> GetPlaylist(string videoUrl)
        {
            return await youTubeDownloader.GetPlaylistSongs(videoUrl);
        }

        public async Task<SongResponse> GetSong(string videoUrl)
        {
            return await youTubeDownloader.GetSingleSong(videoUrl);
        }

        public async Task<DownloadResponse> DownloadAndSave(IEnumerable<string> videoUrls,
            IEnumerable<string> destinationFolders,
            MediaType mediaType=MediaType.Audio)
        {
            return await youTubeDownloader.DownloadAndSaveToDestinationFolder(videoUrls, 
                destinationFolders,
                mediaType);
        }
    }
}
