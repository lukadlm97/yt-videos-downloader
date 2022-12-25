using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VideoAndAudioDownloader.BusinessLogic.Models;
using VideoAndAudioDownloader.Desktop.Commands;
using VideoAndAudioDownloader.Desktop.Models;
using VideoAndAudioDownloader.Desktop.ViewModels;
using VideoAndAudioDownloader.Desktop.ViewModels.Common;

namespace VideoAndAudioDownloader.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private MainWindowViewModel MainWindowViewModel;

        public MainWindow(IStorage storage)
        {
            InitializeComponent();
            MainWindowViewModel = new MainWindowViewModel(storage);
            MainWindowViewModel.PlaylistUrl = "past playlist url";
            MainWindowViewModel.Songs = new ObservableCollection<Song>();
            this.DataContext = MainWindowViewModel;
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
