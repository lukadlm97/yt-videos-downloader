using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VideoAndAudioDownloader.Desktop.Models;

namespace VideoAndAudioDownloader.Desktop.View
{
    public class SelectedItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedItems = (IList)value;
            return new ObservableCollection<PhysicalPath>(selectedItems.OfType<PhysicalPath>());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
