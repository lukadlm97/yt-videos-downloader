using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using VideoAndAudioDownloader.Desktop.Commands;
using System.Windows.Forms;
using VideoAndAudioDownloader.Desktop.Models;
using MessageBox = System.Windows.Forms.MessageBox;

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
            RemoveItemCommand = new RelayCommand(param => RemoveItem(),
                param=>true);
         
            _destinations = new ObservableCollection<PhysicalPath>();
            _selectedItem =null;
        }

        private PhysicalPath _selectedItem;

        public PhysicalPath SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
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
        private void RemoveItem()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Not selected item for removal", "Error occurred");
                return;
            }

            Destinations.Remove(SelectedItem);
        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
