using MediaToolkit.Model;
using MediaToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public class YouTubeDownloader:IYouTubeDownloader
    {
        public async Task<bool> SaveMP3(string videoUrl, string outputFolder =null, CancellationToken cancellationToken=default)
        {
            try
            {
                var youYube = YouTube.Default;
                var video = await youYube.GetVideoAsync(videoUrl);
                var videoFilePath = outputFolder!=null?outputFolder + video.FullName: video.FullName;
                await File.WriteAllBytesAsync(videoFilePath, await video.GetBytesAsync(),
                    cancellationToken);
                var inputFile = new MediaFile { Filename = videoFilePath };
                var outputFile = new MediaFile { Filename = $"{video.FullName}.mp3" };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);

                    engine.Convert(inputFile, outputFile);
                }

                if (File.Exists(Path.Combine(videoFilePath)))
                {
                    File.Delete(Path.Combine(videoFilePath));
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return
                    false;
            }
         
        }
    }
}
