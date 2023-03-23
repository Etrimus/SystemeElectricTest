using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SystemeElectric.DAL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDal(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddDbContext<SystemeElecticDbContext>(opt => opt.UseSqlite("Data Source=_DB.db"), ServiceLifetime.Transient);
    }
}