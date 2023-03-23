using System;

namespace SystemeElectric.Core.Timer;

public class TimerElapsedEventArgs: EventArgs
{
    public TimerElapsedEventArgs(DateTime elapsedTime)
    {
        ElapsedTime = elapsedTime;
    }

    public DateTime ElapsedTime { get; }
}