using SystemeElectic.Core;
using SystemeElectic.Wpf;

namespace SystemeElectic.Runner;

public static class Program
{
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddSingleton<App>()
                    .AddSingleton<MainWindow>()
                    .AddHostedService<TimerBackService>()
                    .AddHostedService<WpfBackService>();
            })
            .Build();

        host.Run();
    }
}