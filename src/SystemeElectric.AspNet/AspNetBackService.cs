using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SystemeElectric.Core;

namespace SystemeElectric.AspNet;

public class AspNetBackService: BackgroundService
{
    private readonly ILogger<AspNetBackService> _logger;
    private readonly WebApplication _app;

    public AspNetBackService(ITimerCoreService timerCoreService, ILogger<AspNetBackService> logger, IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSignalR();

        _app = builder.Build();

        _app.MapHub<SystemeElectricHub>("/hub");

        timerCoreService.DriverArrived += (_, driver) =>
        {
            _app.Services.GetRequiredService<IHubContext<SystemeElectricHub>>().Clients.All.SendAsync("DriverArrived", driver.DateArrived, driver.Name);
        };

        timerCoreService.CarArrived += (_, car) =>
        {
            _app.Services.GetRequiredService<IHubContext<SystemeElectricHub>>().Clients.All.SendAsync("CarArrived", car.DateArrived, car.Model);
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting SignalR service...");
        return _app.RunAsync(stoppingToken);
    }
}