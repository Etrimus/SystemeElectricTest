using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Logging;
using SystemeElectric.Core.Timer;
using SystemeElectric.Entities;

namespace SystemeElectric.Core;

public interface ITimerCoreService: IDisposable
{
    bool IsCarsThreadActive { get; }

    bool IsDriversThreadActive { get; }

    event EventHandler<Car> CarArrived;

    event EventHandler<Driver> DriverArrived;

    event EventHandler<ThreadStateChangedEventArgs> CarsThreadStateChanged;

    event EventHandler<ThreadStateChangedEventArgs> DriversThreadStateChanged;

    void Init(CancellationToken stoppingToken);

    void StartDriversThread();

    void StopDriversThread();

    void StartCarsThread();

    void StopCarsThread();
}

internal class TimerCoreService: ITimerCoreService
{
    private const int TimerInterval = 1000;

    private const int DriverArrivalInterval = 3;
    private const int CarArrivalInterval = 2;

    private static readonly AutoResetEvent AutoEventCars = new(false);
    private static readonly AutoResetEvent AutoEventDrivers = new(false);

    private static DateTime _currentTime;
    private readonly ILogger _logger;
    private readonly IRandomDataService _randomDataService;
    private readonly ITimerService _timerService;

    private bool _isInit;
    private bool _isCarsThreadActive;
    private bool _isDriversThreadActive;

    public TimerCoreService(ILogger logger, IRandomDataService randomDataService, ITimerService timerService)
    {
        _logger = logger;
        _randomDataService = randomDataService;
        _timerService = timerService;
    }

    public bool IsCarsThreadActive
    {
        get => _isCarsThreadActive;
        private set
        {
            _isCarsThreadActive = value;
            CarsThreadStateChanged?.Invoke(this, new ThreadStateChangedEventArgs(value));
        }
    }

    public bool IsDriversThreadActive
    {
        get => _isDriversThreadActive;
        private set
        {
            _isDriversThreadActive = value;
            DriversThreadStateChanged?.Invoke(this, new ThreadStateChangedEventArgs(value));
        }
    }

    #region IDisposable

    public void Dispose()
    {
        StopCarsThread();
        StopDriversThread();
    }

    #endregion

    #region ITimerCoreService

    public void Init(CancellationToken stoppingToken)
    {
        if (_isInit)
        {
            throw new SystemeElectricException("Already initialized.");
        }

        _isInit = true;

        var carsThread = new Thread(_ =>
        {
            while (true)
            {
                AutoEventCars.WaitOne();

                if (!stoppingToken.IsCancellationRequested)
                {
                    CarArrived?.Invoke(this, new Car(_randomDataService.GetRandomCarModel(), _currentTime));
                }
                else
                {
                    return;
                }
            }
        }) { IsBackground = true };

        var driversThread = new Thread(_ =>
        {
            while (true)
            {
                AutoEventDrivers.WaitOne();

                if (!stoppingToken.IsCancellationRequested)
                {
                    DriverArrived?.Invoke(this, new Driver(_randomDataService.GetRandomDriverName(), _currentTime));
                }
                else
                {
                    return;
                }
            }
        }) { IsBackground = true };

        carsThread.Start();
        driversThread.Start();

        _timerService.Elapsed += (_, e) =>
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _timerService.Stop();

                AutoEventCars.Set();
                AutoEventDrivers.Set();

                return;
            }

            _currentTime = e.ElapsedTime;
            var elapsedTime = TimeSpan.FromTicks(e.ElapsedTime.Ticks);

            Debug.WriteLine($"Elapsed time: {Math.Round(elapsedTime.TotalSeconds)}s");

            if (IsCarsThreadActive && Math.Round(elapsedTime.TotalSeconds) % CarArrivalInterval == 0)
            {
                Debug.WriteLine("Car");
                AutoEventCars.Set();
            }

            if (IsDriversThreadActive && Math.Round(elapsedTime.TotalSeconds) % DriverArrivalInterval == 0)
            {
                Debug.WriteLine("Driver");
                AutoEventDrivers.Set();
            }

            Debug.WriteLine(null);
        };

        _timerService.Start(TimerInterval);
    }

    public void StartDriversThread()
    {
        IsDriversThreadActive = true;
        _logger.LogInformation("Drivers thread started");
    }

    public void StopDriversThread()
    {
        IsDriversThreadActive = false;
        _logger.LogInformation("Drivers thread stopped");
    }

    public void StartCarsThread()
    {
        IsCarsThreadActive = true;
        _logger.LogInformation("Cars thread started");
    }

    public void StopCarsThread()
    {
        IsCarsThreadActive = false;
        _logger.LogInformation("Cars thread stopped");
    }

    public event EventHandler<Car> CarArrived;

    public event EventHandler<Driver> DriverArrived;

    public event EventHandler<ThreadStateChangedEventArgs> CarsThreadStateChanged;

    public event EventHandler<ThreadStateChangedEventArgs> DriversThreadStateChanged;

    #endregion
}