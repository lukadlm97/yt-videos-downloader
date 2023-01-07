using System;
using System.Collections.Generic;
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

namespace VideoAndAudioDownloader.Desktop.View
{
    /// <summary>
    /// Interaction logic for AddNewSongUserControl.xaml
    /// </summary>
    public partial class AddNewSongUserControl : UserControl
    {
        public AddNewSongUserControl()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(sender as DependencyObject).DialogResult = false;
        }

        private void AddToExistingPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(sender as DependencyObject).DialogResult = true;
        }
    }
}
