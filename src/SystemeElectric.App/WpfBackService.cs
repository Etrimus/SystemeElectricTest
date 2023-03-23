using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SystemeElectric.App;

public class WpfBackService: BackgroundService
{
    private readonly App _app;
    private readonly ILogger _logger;

    public WpfBackService(App app, ILogger logger)
    {
        _app = app;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting WPF application...");
        _app.Run();
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping WPF application...");
        return base.StopAsync(cancellationToken);
    }
}