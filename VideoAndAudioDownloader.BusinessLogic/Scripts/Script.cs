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
                await _downloader.SaveMP3(videoUrl);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred on import {videoUrl} with exception message: " + ex.Message);
                // return false;
            }

        }
    }
}
