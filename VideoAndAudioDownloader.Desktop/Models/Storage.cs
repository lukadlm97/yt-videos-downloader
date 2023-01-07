using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoAndAudioDownloader.BusinessLogic.Models.DTO;
using VideoAndAudioDownloader.BusinessLogic.Services;

namespace VideoAndAudioDownloader.Desktop.Models
{
    public class Storage:IStorage
    {
        public Storage(IYouTubeDownloader youTubeDownloader)
        {
            this.youTubeDownloader = youTubeDownloader;
        }
        private IEnumerable<Person> Persons = new List<Person>()
        {
            new Person()
            {
                ID = 1,
                FirstName = "Dusan",
                LastName = "Vlahovic",
                Age = 22
            },
            new Person()
            {
                ID = 2,
                FirstName = "Vanja",
                LastName = "Milinkovic-Savic",
                Age = 25
            },
            new Person()
            {
                ID = 3,
                FirstName = "Marko",
                LastName = "Grujic",
                Age = 25
            },
        };

        private readonly IYouTubeDownloader youTubeDownloader;

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            await Task.Delay(1000);
            return Persons;
        }

        public async Task<Person> GetPersonById(int id)
        {
            await Task.Delay(1000);
            return Persons.FirstOrDefault(x=>x.ID==id);
        }

        public async Task<PlaylistResponse> GetPlaylist(string videoUrl)
        {
            return await youTubeDownloader.GetPlaylistSongs(videoUrl);
        }

        public async Task<SongResponse> GetSong(string videoUrl)
        {
            return await youTubeDownloader.GetSingleSong(videoUrl);
        }
    }
}
