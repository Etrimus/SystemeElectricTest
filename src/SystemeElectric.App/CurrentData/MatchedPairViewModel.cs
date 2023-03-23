using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SystemeElectric.App.CurrentData;

public class MatchedPairViewModel: ObservableObject
{
    public MatchedPairViewModel(DateTime datetime, string driverName, string carModel)
    {
        Datetime = datetime;
        DriverName = driverName;
        CarModel = carModel;
    }

    public DateTime Datetime { get; set; }

    public string DriverName { get; set; }

    public string CarModel { get; set; }
}