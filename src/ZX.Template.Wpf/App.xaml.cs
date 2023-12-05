using AntDesign.ProLayout;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using ZX.Template.Core.Extensions;

namespace ZX.Template.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }
        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            ServiceLocator.Instance= ConfigureServices();
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception?.ToString() ?? string.Empty);
            e.Handled = true;
        }

        private  IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
#if DEBUG
            serviceCollection.AddBlazorWebViewDeveloperTools();
#endif

            serviceCollection.AddAntDesign();

            serviceCollection.Configure<ProSettings>(Configuration.GetSection("ProSettings"));

            serviceCollection.AddApp();


            return serviceCollection.BuildServiceProvider();
        }
    }

}
