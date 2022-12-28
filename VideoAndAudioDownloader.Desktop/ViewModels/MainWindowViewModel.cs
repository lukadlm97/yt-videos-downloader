using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConsoleAppFramework;
using VideoAndAudioDownloader.BusinessLogic.Enumerations;
using VideoAndAudioDownloader.BusinessLogic.Models;
using VideoAndAudioDownloader.Desktop.Commands;
using VideoAndAudioDownloader.Desktop.Models;
using VideoAndAudioDownloader.Desktop.ViewModels.Common;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private readonly IStorage storage;
        public MainWindowViewModel(IStorage storage)
        {
            this.storage = storage;
        }
        public string PlaylistUrl { get; set; }

        public ObservableCollection<Song> songs;

   
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
        
        private bool isLoading = false;

        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        private bool isSongsLoaded = false;

        public bool IsSongsLoaded
        {
            get
            {
                return isSongsLoaded;
            }
            set
            {
                isSongsLoaded = value;
                OnPropertyChanged();
            }
        }

        private bool isErrorOccurred;

        public bool IsErrorOccurred
        {
            get
            {
                return isErrorOccurred;
            }
            set
            {
                isErrorOccurred = value;
                OnPropertyChanged();
            }
        }

        private string errorDescription;
        public string ErrorDescription
        {
            get
            {
                return errorDescription;
            }
            set
            {
                errorDescription = value;
                OnPropertyChanged();
            }
        }

        private ICommand loadPlaylistCommand;

        public ICommand LoadPlaylistCommand
        {
            get
            {
                if (loadPlaylistCommand == null)
                {
                    loadPlaylistCommand = new RelayCommand(
                        async param => await LoadAsync(PlaylistUrl),
                        param => true);
                }
                return loadPlaylistCommand;
            }
        }
      //  public bool CanLoad => !(isLoading==Visibility.Hidden);
        
        public async Task LoadAsync(string videoUrl)
        {
            //IsLoading = Visibility.Visible;
            IsLoading = true;
            IsSongsLoaded = false;
            Songs= await GetItemsAsync(videoUrl);
            IsLoading = false;
            IsSongsLoaded = true;
            //IsLoading = Visibility.Hidden;
        }

        async Task<ObservableCollection<Song>> GetItemsAsync(string videoUrl)
        {
            var playlistResponse = await storage.GetPlaylist(videoUrl);
            if (playlistResponse.OperationStatus == OperationStatus.Success)
            {
                return new ObservableCollection<Song>(playlistResponse.Playlist.Songs);
            }

            isErrorOccurred = true;
            errorDescription = playlistResponse.OperationStatus.ToString();
            return new ObservableCollection<Song>();
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
