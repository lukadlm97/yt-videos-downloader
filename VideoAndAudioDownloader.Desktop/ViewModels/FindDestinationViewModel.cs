using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Input;
using VideoAndAudioDownloader.Desktop.Commands;
using System.Windows.Forms;
using VideoAndAudioDownloader.Desktop.Models;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class FindDestinationViewModel : INotifyPropertyChanged
    {
        private string _selectedFolderPath;

        public FindDestinationViewModel()
        {
            SelectFolderCommand = new RelayCommand(
                 param => SelectFolder(),
                param => true);
            RemoveItemCommand = new RelayCommand(param => RemoveItems(), param=>true);
            _destinations = new ObservableCollection<PhysicalPath>();
            _selectedItems = new ObservableCollection<PhysicalPath>();
        }

        private ObservableCollection<PhysicalPath> _selectedItems;

        public ObservableCollection<PhysicalPath> SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                _selectedItems=value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PhysicalPath> _destinations;


        public ObservableCollection<PhysicalPath> Destinations
        {
            get
            {
                return _destinations;
            }
            set
            {
                _destinations = value;
                OnPropertyChanged();
            }
        }


        public ICommand SelectFolderCommand { get; }
        public ICommand RemoveItemCommand { get; }

        public string SelectedFolderPath
        {
            get => _selectedFolderPath;
            set
            {
                _selectedFolderPath = value;
                OnPropertyChanged();
            }
        }

        private void SelectFolder()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var folderName = dialog.SelectedPath;
                var path = dialog.SelectedPath;

                 Destinations.Add(new PhysicalPath()
                 {
                     FolderName = path.Substring(path.LastIndexOf('\\'),path.Length- path.LastIndexOf('\\')),
                     Path = path
                 });
            }
        }
        private void RemoveItems()
        {
            if (SelectedItems == null)
            {
                return;
            }

            foreach (var item in SelectedItems.OfType<PhysicalPath>().ToList())
            {
                Destinations.Remove(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
