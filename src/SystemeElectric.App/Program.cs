using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SystemeElectric.App.FormControls;
using SystemeElectric.App.MainWin;
using SystemeElectric.AspNet;
using SystemeElectric.Core;
using SystemeElectric.DAL;
using SystemeElectric.Logging;

namespace SystemeElectric.App;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        var builder = Host.CreateDefaultBuilder();

        var host = builder
            .ConfigureServices(services =>
            {
                services
                    .AddCore()
                    .AddDal()
                    .AddSingleton<App>()
                    .AddSingleton<MainWindow>()
                    .AddSingleton<FormControlsView>()
                    .AddScoped<MainWindowViewModel>()
                    .AddScoped<IWindowService, WindowService>()
                    .AddHostedService<TimerBackService>()
                    .AddHostedService<DalBackService>()
                    .AddHostedService<AspNetBackService>()
                    .AddHostedService<WpfBackService>();
            })
            .ConfigureLogging(b => b
                .ClearProviders()
                .AddLogging())
            .Build();

        host.Run();
    }
}