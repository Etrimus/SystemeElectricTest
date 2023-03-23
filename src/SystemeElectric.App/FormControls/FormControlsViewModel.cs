using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemeElectric.Core;

namespace SystemeElectric.App.FormControls;

public class FormControlsViewModel: ObservableObject, IDisposable
{
    private readonly ITimerCoreService _timerCoreService;
    private bool _isCarsThreadActive;
    private bool _isDriversThreadActive;

    public FormControlsViewModel(ITimerCoreService timerCoreService, IWindowService windowService)
    {
        _timerCoreService = timerCoreService;

        _timerCoreService.CarsThreadStateChanged += _onCarsThreadStateChanged;
        _timerCoreService.DriversThreadStateChanged += _onDriversThreadStateChanged;

        ToggleCarsThreadCommand = new RelayCommand(() =>
        {
            if (timerCoreService.IsCarsThreadActive)
            {
                timerCoreService.StopCarsThread();
            }
            else
            {
                timerCoreService.StartCarsThread();
            }

            IsCarsThreadActive = timerCoreService.IsCarsThreadActive;
        });

        ToggleDriversThreadCommand = new RelayCommand(() =>
        {
            if (timerCoreService.IsDriversThreadActive)
            {
                timerCoreService.StopDriversThread();
            }
            else
            {
                timerCoreService.StartDriversThread();
            }

            IsDriversThreadActive = timerCoreService.IsDriversThreadActive;
        });

        OpenDatabaseFormCommand = new RelayCommand(windowService.ShowDbDataWindow);
    }

    public ICommand ToggleDriversThreadCommand { get; }

    public ICommand ToggleCarsThreadCommand { get; }

    public ICommand OpenDatabaseFormCommand { get; }

    public bool IsCarsThreadActive
    {
        get => _isCarsThreadActive;
        set => SetProperty(ref _isCarsThreadActive, value);
    }

    public bool IsDriversThreadActive
    {
        get => _isDriversThreadActive;
        set => SetProperty(ref _isDriversThreadActive, value);
    }

    #region IDisposable

    public void Dispose()
    {
        _timerCoreService.CarsThreadStateChanged -= _onCarsThreadStateChanged;
        _timerCoreService.DriversThreadStateChanged -= _onDriversThreadStateChanged;
    }

    #endregion

    private void _onDriversThreadStateChanged(object _, ThreadStateChangedEventArgs args)
    {
        IsDriversThreadActive = args.IsActive;
    }

    private void _onCarsThreadStateChanged(object _, ThreadStateChangedEventArgs args)
    {
        IsCarsThreadActive = args.IsActive;
    }
}