using System;

namespace SystemeElectric.App.MainWin;

public partial class MainWindow: IDisposable
{
    private readonly MainWindowViewModel _mainWindowViewModel;

    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = _mainWindowViewModel = mainWindowViewModel;
    }

    #region IDisposable

    public void Dispose()
    {
        _mainWindowViewModel.Dispose();
    }

    #endregion
}