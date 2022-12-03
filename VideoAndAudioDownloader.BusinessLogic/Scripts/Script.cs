using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using VideoAndAudioDownloader.BusinessLogic.Services;

namespace VideoAndAudioDownloader.BusinessLogic.Scripts
{
    public class Script: ConsoleAppBase,IScript
    {
        private readonly ILogger<Script> _logger;
        private readonly IYouTubeDownloader _downloader;

        public Script(ILogger<Script> logger,IYouTubeDownloader downloader)
        {
            _logger = logger;
            _downloader = downloader;
        }

        [Command("importSingleFile","Command for download single audio file from YT with specified url.")]
        public async Task Import([Option("u","url of yt video")]string videoUrl,
            [Option("o", "output path where yt video should be saved")] string? folderPath=default)
        {
            _logger.LogInformation("Import started for videoUrl: " + videoUrl);

            try
            {
                await _downloader.SaveSingleVideoMP3(videoUrl,false,folderPath, this.Context.CancellationToken);
                _logger.LogInformation("Successfully completed import for video: " + videoUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred on import {videoUrl} with exception message: " + ex.Message);
                // return false;
            }
        }

        [Command("importPlaylist", "Command for download playlist of audio file from YT with specified url.")]
        public async Task ImportPlaylist([Option("u", "url of yt playlist")] string playlistUrl,
            [Option("o", "output path where yt playlist should be saved")] string? folderPath=default,
             [Option("ignorePlaylistName")] bool ignorePlaylistName = default

        )
        {
            _logger.LogInformation("Import started for playlistUrl: " + playlistUrl);

            try
            {
                if (!await _downloader.SavePlaylistMP3(playlistUrl,folderPath,false,this.Context.CancellationToken))
                {
                    _logger.LogError("Not completed import for playlist!!!");
                    return;
                }
                _logger.LogInformation("Successfully completed import for playlist: " + playlistUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred on import {playlistUrl} with exception message: " + ex.Message);
                // return false;
            }
        }
    }
}
