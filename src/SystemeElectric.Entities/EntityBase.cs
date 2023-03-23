using System;

namespace SystemeElectric.Entities;

public abstract class EntityBase
{
    protected EntityBase(DateTime dateArrived)
    {
        DateArrived = dateArrived;
    }

    public Guid Id { get; set; }

    public DateTime DateArrived { get; set; }
}