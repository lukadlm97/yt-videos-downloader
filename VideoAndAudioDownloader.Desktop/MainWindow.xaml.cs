using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoAndAudioDownloader.Desktop.Models;
using VideoAndAudioDownloader.Desktop.ViewModels;
using VideoAndAudioDownloader.Desktop.ViewModels.Common;

namespace VideoAndAudioDownloader.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            DataContext = new MainWindowViewModel()
            {
                NavMenuSelectionStatus = NavMenuSelectionStatus.Home
            };
            InitializeComponent();
        }
    }
}
