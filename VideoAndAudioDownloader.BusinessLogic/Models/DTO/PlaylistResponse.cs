using VideoAndAudioDownloader.BusinessLogic.Enumerations;

namespace VideoAndAudioDownloader.BusinessLogic.Models.DTO;

public class PlaylistResponse
{
    public OperationStatus OperationStatus { get; set; }
    public Playlist Playlist { get; set; }
}