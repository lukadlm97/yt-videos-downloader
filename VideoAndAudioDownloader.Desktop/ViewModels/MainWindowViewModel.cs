using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using VideoAndAudioDownloader.Desktop.Models;
using VideoAndAudioDownloader.Desktop.ViewModels.Common;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class MainWindowViewModel
    {
        public NavMenuSelectionStatus NavMenuSelectionStatus { get; set; }
        public string PlaylistUrl { get; set; }
        public ICommand ButtonCommand { get; set; }
    }
}
