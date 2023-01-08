namespace VideoAndAudioDownloader.BusinessLogic.Scripts;

public interface IScript
{
    Task Import([Option("videoUrl")] string videoUrl, [Option("folderPath")] string? folderPath = default);

    Task ImportPlaylist([Option("playlistUrl")] string playlistUrl, [Option("folderPath")] string? folderPath = default,
        [Option("ignorePlaylistName")] bool ignorePlaylistName = default);
}