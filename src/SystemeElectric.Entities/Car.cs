using System;

namespace SystemeElectric.Entities;

public class Car: EntityBase
{
    public Car(string model, DateTime dateArrived): base(dateArrived)
    {
        Model = model;
    }

    public string Model { get; set; }
}