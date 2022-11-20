using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoAndAudioDownloader.BusinessLogic.Scripts
{
    public interface IScript
    {
        Task Import([Option("videoUrl")]string videoUrl);
    }
}
