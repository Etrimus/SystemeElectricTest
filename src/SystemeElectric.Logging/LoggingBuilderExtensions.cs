using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SystemeElectric.Logging;

public static class LoggingBuilderExtensions
{
    public static IServiceCollection AddLogging(this ILoggingBuilder builder)
    {
        return builder.Services.AddScoped<ILogger, SystemeElectricLogger>();
    }
}