namespace VideoAndAudioDownloader.BusinessLogic.Models;

public class Playlist
{
    public string Name { get; set; }
    public IEnumerable<Song> Songs { get; set; }
}