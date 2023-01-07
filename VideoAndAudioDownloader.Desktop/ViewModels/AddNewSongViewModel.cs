using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ConsoleAppFramework;
using VideoAndAudioDownloader.BusinessLogic.Enumerations;
using VideoAndAudioDownloader.BusinessLogic.Models;
using VideoAndAudioDownloader.Desktop.Commands;
using VideoAndAudioDownloader.Desktop.Models;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class AddNewSongViewModel:INotifyPropertyChanged
    {
        public AddNewSongViewModel(IStorage storage)
        {
            this._storage = storage;
        }

        private ICommand _searchUrlCommand;

        public ICommand SearchUrlCommand
        {
            get
            {
                if (_searchUrlCommand is null)
                {
                    _searchUrlCommand = new RelayCommand(async param =>
                        await PreformSearchUrl(), param => true);
                }

                return _searchUrlCommand;
            }
        }

        private async Task PreformSearchUrl()
        {
            var songResponse = await _storage.GetSong(_searchUrl);
            if (songResponse.OperationStatus == OperationStatus.Success)
            {
                Song=songResponse.Song;
                IsPlaylistLoaded = false;
                return;
            }

            var playlistResponse = await _storage.GetPlaylist(_searchUrl);
            if (playlistResponse.OperationStatus == OperationStatus.Success)
            {
                IsPlaylistLoaded = true;
                Playlist=playlistResponse.Playlist;
                return;
            }

            OperationDescription = $"Search for {_searchUrl} was finished with error. Check url please, at your browser!";
            return;
            
        }

        private string _searchUrl;

        public string SearchUrl
        {
            get
            {
                return _searchUrl;
            }
            set
            {
                _searchUrl=value;
                OnPropertyChanged();
            }
        }

        private Playlist _playlist;

        public Playlist Playlist
        {
            get
            {
                return _playlist;
            }
            set
            {
                _playlist = value;
                OnPropertyChanged();
                OperationDescription = $"Playlist fetched :{_playlist.Name} is founded. Do you want to add all songs to playlist?";
            }
        }
        private Song _song;

        public Song Song
        {
            get
            {
                return _song;
            }
            set
            {
                _song=value;
                OnPropertyChanged();
                OperationDescription = $"Song:{_song.Title} is founded. Do you want to add her to playlist?";
            }
        }

        private readonly IStorage _storage;
        private bool _isPlaylistLoaded;

        public bool IsPlaylistLoaded
        {
            get
            {
                return _isPlaylistLoaded;
            }
            set
            {
                _isPlaylistLoaded=value;
                OnPropertyChanged();
            }
        }

        private string _operationsDescription;
        public string OperationDescription
        {
            get
            {
                return _operationsDescription;
            }
            set
            {
                _operationsDescription = value;
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
