using System;
using System.Collections.Generic;

namespace SystemeElectric.Core;

internal interface IRandomDataService
{
    string GetRandomCarModel();

    string GetRandomDriverName();
}

internal class RandomDataService: IRandomDataService
{
    private static readonly string[] CarNames = { "Mazda 6", "Ford Mondeo", "Toyota Camry", "Hyundai Sonata", "Kia K5" };
    private static readonly string[] DriverNames = { "Driver 1", "Driver 2", "Driver 3", "Driver 4", "Driver 5", "Driver 6" };

    #region IRandomDataService

    public string GetRandomCarModel()
    {
        return _getRandomElement(CarNames);
    }

    public string GetRandomDriverName()
    {
        return _getRandomElement(DriverNames);
    }

    #endregion

    private static T _getRandomElement<T>(IReadOnlyList<T> elements)
    {
        return elements[new Random().Next(0, elements.Count - 1)];
    }
}