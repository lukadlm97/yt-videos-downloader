using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public interface IYouTubeDownloader
    {
        Task<bool> SaveSingleVideoMP3(string videoUrl, string outputFolder = null, CancellationToken cancellationToken = default);
        Task<bool> SavePlaylistMP3(string videoUrl, string outputFolder = null, CancellationToken cancellationToken = default);
    }
}
