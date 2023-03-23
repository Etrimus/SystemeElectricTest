using System;

namespace SystemeElectric.Core;

public class ThreadStateChangedEventArgs: EventArgs
{
    public ThreadStateChangedEventArgs(bool isActive)
    {
        IsActive = isActive;
    }

    public bool IsActive { get; }
}