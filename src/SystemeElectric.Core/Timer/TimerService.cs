using System;

namespace SystemeElectric.Core.Timer;

public interface ITimerService
{
    public event EventHandler<TimerElapsedEventArgs> Elapsed;

    public void Start(double interval);

    public void Stop();
}

internal class TimerService: ITimerService
{
    private readonly System.Timers.Timer _timer;

    public TimerService()
    {
        _timer = new System.Timers.Timer();

        _timer.Elapsed += (sender, args) => Elapsed?.Invoke(sender, new TimerElapsedEventArgs(args.SignalTime));
    }

    #region ITimerService

    public event EventHandler<TimerElapsedEventArgs> Elapsed;

    public void Start(double interval)
    {
        _timer.Interval = interval;
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    #endregion
}