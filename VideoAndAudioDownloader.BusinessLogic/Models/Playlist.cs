using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace VideoAndAudioDownloader.BusinessLogic.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        public IEnumerable<Song> Songs { get; set; }
    } 
}
