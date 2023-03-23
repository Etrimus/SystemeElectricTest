using Microsoft.EntityFrameworkCore;
using SystemeElectric.Entities;

namespace SystemeElectric.DAL;

public class SystemeElecticDbContext: DbContext
{
    public SystemeElecticDbContext(DbContextOptions<SystemeElecticDbContext> options): base(options)
    { }

    public DbSet<Car> Cars { get; set; }

    public DbSet<Driver> Drivers { get; set; }
}