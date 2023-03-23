using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SystemeElectric.App.DbData;

public class DbDataRowViewModel: ObservableObject
{
    public DbDataRowViewModel(DateTime dateArrived, string title)
    {
        DateArrived = dateArrived;
        Title = title;
    }

    public DateTime DateArrived { get; set; }

    public string Title { get; set; }
}