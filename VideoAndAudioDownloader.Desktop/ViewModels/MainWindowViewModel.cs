using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using VideoAndAudioDownloader.BusinessLogic.Models;
using VideoAndAudioDownloader.Desktop.Models;
using VideoAndAudioDownloader.Desktop.ViewModels.Common;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        public MainWindowViewModel(IStorage storage)
        {
            this.storage = storage;
        }
        public NavMenuSelectionStatus NavMenuSelectionStatus { get; set; }
        public string PlaylistUrl { get; set; }
        public ICommand ButtonCommand { get; set; }

        public ObservableCollection<Song> songs;
        private readonly IStorage storage;

   
        public ObservableCollection<Song> Songs
        {
            get
            {
                return songs;
            }
            set
            {
                songs = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
