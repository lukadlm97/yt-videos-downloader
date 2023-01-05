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
using System.Windows.Shapes;
using VideoAndAudioDownloader.Desktop.ViewModels;

namespace VideoAndAudioDownloader.Desktop.View
{
    /// <summary>
    /// Interaction logic for FindDestinationWindow.xaml
    /// </summary>
    public partial class FindDestinationWindow : Window
    {
        private FindDestinationViewModel FindDestinationViewModel;
        public FindDestinationWindow()
        {
            InitializeComponent();
            FindDestinationViewModel = new FindDestinationViewModel();
            this.DataContext= FindDestinationViewModel;
        }
    }
}
