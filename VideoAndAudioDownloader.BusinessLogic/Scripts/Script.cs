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

        public async Task Import(string videoUrl)
        {
            _logger.LogInformation("Import started for videoUrl: " + videoUrl);

            try
            {
                await _downloader.SaveSingleVideoMP3(videoUrl);
                _logger.LogInformation("Successfully completed import for video: " + videoUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred on import {videoUrl} with exception message: " + ex.Message);
                // return false;
            }
        }

        public async Task ImportPlaylist(string playlistUrl)
        {
            _logger.LogInformation("Import started for videoUrl: " + playlistUrl);

            try
            {
                if (!await _downloader.SavePlaylistMP3(playlistUrl))
                {
                    _logger.LogError("Not completed import!!!");
                    return;
                }
                _logger.LogInformation("Successfully completed import for video: " + playlistUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred on import {playlistUrl} with exception message: " + ex.Message);
                // return false;
            }
        }
    }
}
