using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SystemeElectric.Logging;

internal class SystemeElectricLogger: ILogger
{
    private const string LogFolderPath = "log";
    private const string LogFileName = "log.txt";
    private const int FileCreatingInterval = 2;

    #region ILogger

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (logLevel == LogLevel.None)
        {
            return;
        }

        Directory.CreateDirectory(LogFolderPath);
        File.AppendAllText(_getLogFilePath(), $"[{logLevel.ToString().ToUpperInvariant()}]: {formatter(state, exception)}{Environment.NewLine}");
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state) where TState: notnull
    {
        throw new NotSupportedException();
    }

    #endregion

    private static string _getLogFilePath()
    {
        return Path.Combine(LogFolderPath, $"{DateTime.Now:yyyy-MM-dd_HH}-{_getIntervalStartingValue(FileCreatingInterval):00}_{LogFileName}");
    }

    private static int _getIntervalStartingValue(int interval)
    {
        if (!_between(interval, 0, 60))
        {
            throw new ArgumentException($"{nameof(interval)} value is not actual value of minutes.");
        }

        var segmentsCount = (int)Math.Round(60.0 / interval, MidpointRounding.ToPositiveInfinity);

        var minutes = DateTime.UtcNow.Minute;

        for (var i = 0; i < segmentsCount; i++)
        {
            var intervalStart = interval * i;
            var intervalEnd = _clamp(intervalStart + interval, 60) - 1;

            Debug.WriteLine($"{intervalStart:00} : {intervalEnd:00}");

            if (_between(minutes, intervalStart, intervalEnd))
            {
                return intervalStart;
            }
        }

        return 0;
    }

    private static bool _between(int value, int from, int to)
    {
        return value >= from && value <= to;
    }

    private static int _clamp(int value, int clampTo)
    {
        return value <= clampTo ? value : clampTo;
    }
}