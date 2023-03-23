using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SystemeElectric.Core;

public class TimerBackService: BackgroundService
{
    private readonly ITimerCoreService _timerCoreService;

    public TimerBackService(ITimerCoreService timerCoreService)
    {
        _timerCoreService = timerCoreService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timerCoreService.Init(stoppingToken);

        _timerCoreService.StartCarsThread();
        _timerCoreService.StartDriversThread();

        return Task.CompletedTask;
    }
}