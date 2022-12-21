using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using VideoAndAudioDownloader.BusinessLogic.Services;
using VideoAndAudioDownloader.Desktop.Models;
using VideoAndAudioDownloader.Desktop.ViewModels;

namespace VideoAndAudioDownloader.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorage, Storage>();
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<IYouTubeDownloader, ExplodeYouTubeDownloader>();
        }

        protected  void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
        
    }
}
