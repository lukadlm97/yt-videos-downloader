using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VideoAndAudioDownloader.Desktop.Models;

namespace VideoAndAudioDownloader.Desktop.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(ObservableCollection<PersonViewModel> personsViewModel)
        {
            PersonsViewModel = personsViewModel;
        }

        async public static Task<MainWindowViewModel> BuildViewModelAsync(IStorage storage)
        {
            ObservableCollection<PersonViewModel> tempData = new ObservableCollection<PersonViewModel>((await storage.GetAllPersons()).Select(x=>new PersonViewModel()
            {
                Age = x.Age,
                Name = x.FirstName+" "+x.LastName
            }));
            return new MainWindowViewModel(tempData);
        }

        public ObservableCollection<PersonViewModel> PersonsViewModel { get; set; }
    }
}
