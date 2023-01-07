using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VideoAndAudioDownloader.BusinessLogic.Models;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class AddNewSongViewModel:INotifyPropertyChanged
    {
        public AddNewSongViewModel()
        {
            
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
        public string OperationDescription { get; set; } = "Message after preformed search";

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
