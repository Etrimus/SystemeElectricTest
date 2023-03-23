using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SystemeElectric.App.CurrentData;
using SystemeElectric.App.FormControls;
using SystemeElectric.Core;

namespace SystemeElectric.App.MainWin;

public class MainWindowViewModel: ObservableObject, IDisposable
{
    public MainWindowViewModel(ITimerCoreService timerCoreService, IWindowService windowService)
    {
        FormControlsViewModel = new FormControlsViewModel(timerCoreService, windowService);
        CurrentDataViewModel = new CurrentDataViewModel(timerCoreService);
    }

    public FormControlsViewModel FormControlsViewModel { get; }

    public CurrentDataViewModel CurrentDataViewModel { get; }

    #region IDisposable

    public void Dispose()
    {
        FormControlsViewModel.Dispose();
        CurrentDataViewModel.Dispose();
    }

    #endregion
}