using Microsoft.Extensions.DependencyInjection;
using SystemeElectric.Core.Timer;

namespace SystemeElectric.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<ITimerService, TimerService>()
            .AddSingleton<ITimerCoreService, TimerCoreService>()
            .AddScoped<IRandomDataService, RandomDataService>();
    }
}