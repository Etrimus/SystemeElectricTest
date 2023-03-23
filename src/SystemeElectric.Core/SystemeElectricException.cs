using System;

namespace SystemeElectric.Core;

public class SystemeElectricException: Exception
{
    public SystemeElectricException(string message)
    {
        Message = message;
    }

    public SystemeElectricException(string message, Exception innerException)
    {
        Message = message;
        InnerException = innerException;
    }

    public string Message { get; }

    public Exception InnerException { get; }
}