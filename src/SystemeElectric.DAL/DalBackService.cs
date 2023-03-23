using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemeElectric.Core;

namespace SystemeElectric.DAL;

public class DalBackService: BackgroundService
{
    private readonly ITimerCoreService _timerCoreService;
    private readonly IServiceProvider _serviceProvider;

    public DalBackService(ITimerCoreService timerCoreService, IServiceProvider serviceProvider)
    {
        _timerCoreService = timerCoreService;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _getDbContext().Database.EnsureCreated();

        _timerCoreService.CarArrived += async (_, car) =>
        {
            var db = _getDbContext();
            await db.Cars.AddAsync(car, stoppingToken);
            await db.SaveChangesAsync(stoppingToken);
        };

        _timerCoreService.DriverArrived += async (_, driver) =>
        {
            var db = _getDbContext();
            await db.Drivers.AddAsync(driver, stoppingToken);
            await db.SaveChangesAsync(stoppingToken);
        };

        return Task.CompletedTask;
    }

    private SystemeElecticDbContext _getDbContext()
    {
        return _serviceProvider.GetRequiredService<SystemeElecticDbContext>();
    }
}