using System;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SystemeElectric.Core;
using SystemeElectric.Entities;

namespace SystemeElectric.App.CurrentData;

public class CurrentDataViewModel: ObservableObject, IDisposable
{
    private readonly ITimerCoreService _timerCoreService;
    private Car _lastArrivedCar;
    private Driver _lastArrivedDriver;

    public CurrentDataViewModel(ITimerCoreService timerCoreService)
    {
        _timerCoreService = timerCoreService;

        _timerCoreService.CarArrived += _onCarArrived;
        _timerCoreService.DriverArrived += _onDriverArrived;
    }

    public ObservableCollection<MatchedPairViewModel> MatchedPairs { get; } = new();

    #region IDisposable

    public void Dispose()
    {
        _timerCoreService.CarArrived -= _onCarArrived;
        _timerCoreService.DriverArrived -= _onDriverArrived;
    }

    #endregion

    private void _onDriverArrived(object _, Driver driver)
    {
        _lastArrivedDriver = driver;
        _tryAddMatchedPair();
    }

    private void _onCarArrived(object _, Car car)
    {
        _lastArrivedCar = car;
        _tryAddMatchedPair();
    }

    private void _tryAddMatchedPair()
    {
        if (_lastArrivedCar != null && _lastArrivedDriver != null && _lastArrivedCar.DateArrived == _lastArrivedDriver.DateArrived)
        {
            Application.Current.Dispatcher
                .Invoke(() => MatchedPairs.Add(new MatchedPairViewModel(_lastArrivedCar.DateArrived, _lastArrivedDriver.Name, _lastArrivedCar.Model)));
        }
    }
}