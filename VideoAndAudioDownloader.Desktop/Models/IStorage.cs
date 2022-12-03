using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoAndAudioDownloader.Desktop.Models
{
    public interface IStorage
    {
        Task<IEnumerable<Person>> GetAllPersons();
        Task<Person> GetPersonById(int id);

    }
}
