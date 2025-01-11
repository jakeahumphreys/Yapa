using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Photino.Blazor;
using Yapa.Data;
using Yapa.Features.NoteTaking;

namespace Yapa
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
            
            var appDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Yapa\\";
            
            if(!Directory.Exists(appDirectory))
                Directory.CreateDirectory(appDirectory);
            
            var databaseFile = Path.Combine(appDirectory, "datastore.db");
            
            if(!File.Exists(databaseFile))
                File.Create(databaseFile).Dispose();
            
            
            var sessionFactory = NHibernateConfig.CreateSessionFactory(databaseFile);

            appBuilder.Services
                .AddLogging()
                .AddMudServices(config =>
                {
                    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                    config.SnackbarConfiguration.PreventDuplicates = true;
                    config.SnackbarConfiguration.NewestOnTop = false;
                    config.SnackbarConfiguration.ShowCloseIcon = true;
                    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
                    config.SnackbarConfiguration.VisibleStateDuration = 5000;
                    config.SnackbarConfiguration.HideTransitionDuration = 500;
                    config.SnackbarConfiguration.ShowTransitionDuration = 500;
                })
                .AddSingleton(sessionFactory)
                .AddSingleton<INoteRepository, NoteRepository>()
                .AddSingleton<ICollectionRepository, CollectionRepository>()
                .AddSingleton<NoteService>()
                .AddSingleton<CollectionService>()
                .AddSingleton(TimeProvider.System);

            // register root component and selector
            appBuilder.RootComponents.Add<App>("app");

            var app = appBuilder.Build();

            // customize window
            app.MainWindow
                .SetIconFile("favicon.ico")
                .SetTitle("Yapa");

            AppDomain.CurrentDomain.UnhandledException += (sender, error) => { app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString()); };
            
            app.Run();
            
            sessionFactory?.Dispose();
        }
    }
}