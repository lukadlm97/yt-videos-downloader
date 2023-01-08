using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using MessageBox = System.Windows.Forms.MessageBox;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private readonly IYouTubeService storage;
        public MainWindowViewModel(IYouTubeService storage)
        {
            this.storage = storage;
            Paths = new List<PhysicalPath>();
        }

        private string _searchPlaylistUrl;

        public string SearchPlaylistUrl
        {
            get
            {
                return _searchPlaylistUrl;
            }
            set
            {
                _searchPlaylistUrl = value;
                OnPropertyChanged();
            }
        }
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
                        async param => await LoadAsync(SearchPlaylistUrl),
                        param => true);
                }
                return loadPlaylistCommand;
            }
        }

        private bool _isNewFolderSelected;

        public bool IsNewFolderSelected
        {
            get
            {
                return _isNewFolderSelected;
            }
            set
            {
                _isNewFolderSelected= value;
                OnPropertyChanged();
            }
        }
        private bool _isVideoSelected;

        public bool IsVideoSelected
        {
            get
            {
                return _isVideoSelected;
            }
            set
            {
                _isVideoSelected = value;
                OnPropertyChanged();
            }
        }
        

        private string _folderName="folder-name";

        public string FolderName
        {
            get
            {
                return _folderName;
            }
            set
            {
                _folderName = value;
                OnPropertyChanged();
            }
        }

        private bool _invertedIsSongLoaded = true;

        public bool InvertedIsSongLoaded
        {
            get
            {
                return _invertedIsSongLoaded;
            }
            set
            {
                _invertedIsSongLoaded = value;
                OnPropertyChanged();
            }
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

        private ICommand _updateSearchCommand;

        public ICommand UpdateSearchCommand
        {
            get
            {
                if (_updateSearchCommand == null)
                {
                    _updateSearchCommand = new RelayCommand(
                        param => UpdateSearch(),
                        param => true);
                }
                return _updateSearchCommand;
            }
        }

        private void UpdateSearch()
        {
            IsSongsLoaded = false;
            InvertedIsSongLoaded = true;
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
        private ICommand _returnOnPlaylistTweaking;

        public ICommand ReturnOnPlaylistTweaking
        {
            get
            {
                if (_returnOnPlaylistTweaking == null)
                {

                    _returnOnPlaylistTweaking = new RelayCommand(
                        param => ReturnOnPlaylist(),
                        param => true);
                }

                return _returnOnPlaylistTweaking;
            }
        }
        private ICommand _saveSelectedVideosToDestinationFolderCommand;

        public ICommand SaveSelectedVideosToDestinationFolderCommand
        {
          get{
                if (_saveSelectedVideosToDestinationFolderCommand == null)
                {
                    _saveSelectedVideosToDestinationFolderCommand = 
                        new RelayCommand(async param => await DownloadAndSave(),
                        param => true);
                }

                return _saveSelectedVideosToDestinationFolderCommand;
            }
        }


        public async Task DownloadAndSave()
        {
            FullWindowOperationOccurred = false;
            var paths = Paths.Select(x => x.Path);
            if (IsNewFolderSelected)
            {
                paths = paths.Select(x => x + "\\" + FolderName);
            }
            var response = await storage.DownloadAndSave(Songs.Select(x => x.Url),
                paths,IsVideoSelected?MediaType.Video:MediaType.Audio);
            if (response.OperationStatus == OperationStatus.Success)
            {

                MessageBox.Show( "Download and save to destination system has successfully finished!!!", "Error occurred");
                ReinizializationOfView();
                FullWindowOperationOccurred = true;
                return;
            }

            if (response.OperationStatus == OperationStatus.MissingParts)
            {
                MessageBox.Show("Some of videos or destination folders are not available now.", "Error occurred");
                FullWindowOperationOccurred = true; 
                return;
            }

            MessageBox.Show( "Some network error occurred.", "Error occurred");
            FullWindowOperationOccurred = true;

        }

        private bool _fullWindowOperationOccurred = true;

        public bool FullWindowOperationOccurred
        {
            get
            {
                return _fullWindowOperationOccurred;
            }
            set
            {
                _fullWindowOperationOccurred= value;
                OnPropertyChanged();
            }
        }

        private void ReinizializationOfView()
        {
            Songs = new ObservableCollection<Song>();
            Paths = new List<PhysicalPath>();
            SelectedSong = null;
            SearchPlaylistUrl=string.Empty;
            IsNewFolderSelected = false;
            IsVideoSelected=false;
        }

        public void RemoveSongFromList()
        {
            if (Songs.Contains(SelectedSong))
            {
                Songs.Remove(SelectedSong);
                return;
            }

            MessageBox.Show("Not selected any song from list!", "Error occurred");
        }

        public void OpenAddNewSongWindow()
        {
            var rawAddNewSongViewModel = new AddNewSongViewModel(storage)
            {
                SearchUrl = "copy there url of source"
            };
                Window addNewSongWindow = new Window()
            {
                Content = new AddNewSongUserControl(),
                Title = "Add new song to playlist",
                DataContext = rawAddNewSongViewModel,
                Width = 850,
                Height = 500
                };
            addNewSongWindow.ShowDialog();
            if (addNewSongWindow.DialogResult is null)
            {
                return;
            }

            if (addNewSongWindow.DialogResult is false)
            {
                return;
            }
            var addNewSongView = (AddNewSongUserControl)addNewSongWindow.Content;
            var addNewSongViewModel = (AddNewSongViewModel)addNewSongView.DataContext;
            
            if (addNewSongViewModel.IsPlaylistLoaded)
            {
                foreach (var song in addNewSongViewModel.Playlist.Songs)
                {
                    Songs.Add(song);
                }
                return;
            }
            Songs.Add(addNewSongViewModel.Song);
        }
        public void OpenFindDestinationFolder()
        {
           Window findDestinationWindow = new Window()
            {
                Width = 650,
                Height = 400,
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
            InvertedIsSongLoaded = false;
            //IsLoading = Visibility.Hidden;
        }

        private void ReturnOnPlaylist()
        {
            IsSongsLoaded = true;
            InvertedIsSongLoaded = false;
        }

        async Task<ObservableCollection<Song>> GetItemsAsync(string videoUrl)
        {
            var playlistResponse = await storage.GetPlaylist(videoUrl);
            if (playlistResponse.OperationStatus == OperationStatus.Success)
            {
                return new ObservableCollection<Song>(playlistResponse.Playlist.Songs);
            }

            MessageBox.Show("Playlist is not loaded.Reasons: "+ playlistResponse.OperationStatus.ToString(), "Error occurred");
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
