using Microsoft.Extensions.DependencyInjection;
using pi_client.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace pi_client.Views
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
            private IServiceProvider _serviceProvider;

            protected override void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);

                var services = new ServiceCollection();
                ConfigureServices(services);

                _serviceProvider = services.BuildServiceProvider();

                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }

            private void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            }
        }
    
}
