using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using VideoAndAudioDownloader.BusinessLogic.Enumerations;
using VideoAndAudioDownloader.BusinessLogic.Models;
using VideoAndAudioDownloader.Desktop.Commands;
using VideoAndAudioDownloader.Desktop.Models;
using VideoAndAudioDownloader.Desktop.View;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private readonly IStorage storage;
        public MainWindowViewModel(IStorage storage)
        {
            this.storage = storage;
            Paths = new List<PhysicalPath>();
        }
        public string PlaylistUrl { get; set; }
        private Song _selectedSong { get; set; }

        public Song SelectedSong
        {
            get
            {
                return _selectedSong;
            }
            set
            {
                _selectedSong = value;
                OnPropertyChanged();
            }
        }

        private IList<PhysicalPath> _paths;

        public IList<PhysicalPath> Paths
        {
            get
            {
                return _paths;
            }
            set
            {
                _paths = value;
                OnPropertyChanged();
            }
        }
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

        public bool InvertedIsSongLoaded
        {
            get { return !IsSongsLoaded; }
        }

        private ICommand findDestinationFolderCommand;

        public ICommand FindDestinationFolderCommand
        {
            get
            {
                if (findDestinationFolderCommand == null)
                {
                    findDestinationFolderCommand = new RelayCommand(
                         param => OpenFindDestinationFolder(),
                        param => true);
                }
                return findDestinationFolderCommand;
            }
        }

        private ICommand _removeSongFromListCommand;

        public ICommand RemoveSongFromListCommand
        {
            get
            {
                if (_removeSongFromListCommand == null)
                {
               
                        _removeSongFromListCommand = new RelayCommand(
                            param => RemoveSongFromList(),
                            param => true);
                }

                return _removeSongFromListCommand;
            }
        }
        private ICommand _openAddNewSongCommand;

        public ICommand OpenAddNewSongCommand
        {
            get
            {
                if (_openAddNewSongCommand == null)
                {

                    _openAddNewSongCommand = new RelayCommand(
                        param => OpenAddNewSongWindow(),
                        param => true);
                }

                return _openAddNewSongCommand;
            }
        }

        public void RemoveSongFromList()
        {
            if (Songs.Contains(SelectedSong))
            {
                Songs.Remove(SelectedSong);
            }
        }

        public void OpenAddNewSongWindow()
        {
            Window addNewSongWindow = new Window()
            {
                Content = new AddNewSongUserControl(),
                Title = "Add new song to playlist",
                DataContext = new AddNewSongViewModel()
                {
                    SearchUrl = "copy there url of source"
                }
            };
            addNewSongWindow.ShowDialog();

        }
        public void OpenFindDestinationFolder()
        {
           Window findDestinationWindow = new Window()
            {
                Content = new FindDestinationWindow(),
                Title = "Find destination folders",
                DataContext = new FindDestinationViewModel()
                {
                    Destinations = new ObservableCollection<PhysicalPath>(Paths)
                }
            };
            findDestinationWindow.ShowDialog();

            var findDestinationView = (FindDestinationWindow)findDestinationWindow.Content;
            var findDestinationViewModel = (FindDestinationViewModel)findDestinationView.DataContext;

            Paths = findDestinationViewModel.Destinations;
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
