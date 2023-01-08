using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoAndAudioDownloader.BusinessLogic.Models.DTO;

namespace VideoAndAudioDownloader.Desktop.Models
{
    public interface IStorage
    {
        Task<IEnumerable<Person>> GetAllPersons();
        Task<Person> GetPersonById(int id);
        Task<PlaylistResponse> GetPlaylist(string videoUrl);
        Task<SongResponse> GetSong(string videoUrl);
        Task<DownloadResponse> DownloadAndSave(IEnumerable<string> videoUrls,IEnumerable<string> destinationFolders);

    }
}
