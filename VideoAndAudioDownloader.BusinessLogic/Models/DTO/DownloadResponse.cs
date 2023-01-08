using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoAndAudioDownloader.BusinessLogic.Enumerations;

namespace VideoAndAudioDownloader.BusinessLogic.Models.DTO
{
    public class DownloadResponse
    {
        public OperationStatus OperationStatus { get; set; }
    }
}
