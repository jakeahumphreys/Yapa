using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
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
                .AddMudServices()
                .AddSingleton(sessionFactory)
                .AddSingleton<INoteRepository, NoteRepository>()
                .AddSingleton<ICollectionRepository, CollectionRepository>()
                .AddSingleton<NoteService>()
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