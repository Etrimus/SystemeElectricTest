using System;

namespace SystemeElectric.Entities;

public class Driver: EntityBase
{
    public Driver(string name, DateTime dateArrived): base(dateArrived)
    {
        Name = name;
    }

    public string Name { get; set; }
}