using VideoAndAudioDownloader.BusinessLogic.Enumerations;

namespace VideoAndAudioDownloader.BusinessLogic.Models.DTO;

public class SongResponse
{
    public OperationStatus OperationStatus { get; set; }
    public Song Song { get; set; }
}