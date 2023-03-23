using System;
using System.Windows;
using Microsoft.Extensions.Logging;
using SystemeElectric.App.DbData;
using SystemeElectric.DAL;

namespace SystemeElectric.App;

public interface IWindowService
{
    public void ShowDbDataWindow();
}

internal class WindowService: IWindowService
{
    private readonly SystemeElecticDbContext _dbContext;
    private readonly ILogger _logger;

    public WindowService(SystemeElecticDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    #region IWindowService

    public void ShowDbDataWindow()
    {
        var window = new DbDataWindow
        {
            DataContext = new DbDataViewModel(_dbContext),
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        window.Closed += (_, _) =>
        {
            ((IDisposable)window.DataContext)?.Dispose();
            _logger.LogInformation("Database form closed");
        };

        window.Show();
        _logger.LogInformation("Database form opened");
    }

    #endregion
}