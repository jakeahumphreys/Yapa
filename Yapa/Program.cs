using System;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Photino.Blazor;

namespace Yapa
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

            appBuilder.Services
                .AddLogging()
                .AddMudServices();

            // register root component and selector
            appBuilder.RootComponents.Add<App>("app");

            var app = appBuilder.Build();

            // customize window
            app.MainWindow
                .SetIconFile("favicon.ico")
                .SetTitle("Yapa");

            AppDomain.CurrentDomain.UnhandledException += (sender, error) => { app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString()); };

            app.Run();
        }
    }
}