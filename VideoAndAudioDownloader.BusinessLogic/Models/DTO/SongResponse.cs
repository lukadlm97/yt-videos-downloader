using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoAndAudioDownloader.BusinessLogic.Enumerations;

namespace VideoAndAudioDownloader.BusinessLogic.Models.DTO
{
    public class SongResponse
    {
        public OperationStatus OperationStatus { get; set; }
        public Song Song { get; set; }
    }
}
