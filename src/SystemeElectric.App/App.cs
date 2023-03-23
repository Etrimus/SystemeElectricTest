using System.Windows;
using Microsoft.Extensions.Hosting;
using SystemeElectric.App.MainWin;

namespace SystemeElectric.App;

public class App: Application
{
    public App(MainWindow window, IHostApplicationLifetime hostApplicationLifetime)
    {
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        MainWindow = window;

        Exit += (_, _) =>
        {
            window.Dispose();
            hostApplicationLifetime.StopApplication();
        };
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow!.Show();
    }
}