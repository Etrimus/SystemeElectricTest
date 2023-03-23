using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using SystemeElectric.DAL;
using SystemeElectric.Entities;

namespace SystemeElectric.App.DbData;

public class DbDataViewModel: ObservableObject, IDisposable
{
    private readonly SystemeElecticDbContext _dbContext;
    private readonly Timer _timer;

    public DbDataViewModel(SystemeElecticDbContext dbContext)
    {
        _dbContext = dbContext;

        _timer = new Timer(1000);
        _timer.Elapsed += OnTimerOnElapsed;
        _timer.Enabled = true;
        _timer.AutoReset = true;
        _timer.Start();
    }

    public ObservableCollection<DbDataRowViewModel> DataRows { get; } = new();

    #region IDisposable

    public void Dispose()
    {
        _timer.Elapsed -= OnTimerOnElapsed;
        _timer.Stop();
    }

    #endregion

    private async void OnTimerOnElapsed(object o, ElapsedEventArgs elapsedEventArgs)
    {
        _timer.Elapsed -= OnTimerOnElapsed;

        var latestDateArrived = DataRows.FirstOrDefault()?.DateArrived;

        var cars = await _getQuery(_dbContext.Cars, latestDateArrived).ToArrayAsync();
        var drivers = await _getQuery(_dbContext.Drivers, latestDateArrived).ToArrayAsync();

        var dataRows = cars.Select(x => new DbDataRowViewModel(x.DateArrived, x.Model))
            .Concat(drivers.Select(x => new DbDataRowViewModel(x.DateArrived, x.Name)))
            .OrderBy(x => x.DateArrived);

        Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (var dataRow in dataRows)
            {
                DataRows.Insert(0, dataRow);
            }
        });

        _timer.Elapsed += OnTimerOnElapsed;
    }

    private static IQueryable<T> _getQuery<T>(IQueryable<T> dbSet, DateTime? latestDateArrived) where T: EntityBase
    {
        return latestDateArrived.HasValue
            ? dbSet.Where(x => x.DateArrived > latestDateArrived.Value)
            : dbSet;
    }
}