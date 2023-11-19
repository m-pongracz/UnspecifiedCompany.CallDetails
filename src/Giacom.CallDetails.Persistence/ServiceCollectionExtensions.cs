using Giacom.CallDetails.Persistence.CallDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Giacom.CallDetails.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CallDetailsDbContext>(options => options
            .UseSqlServer(configuration["Database:ConnectionString"],
                x => x.MigrationsAssembly(MigrationConstants.MigrationAssembly)));
        
        services.AddScoped<ICallDetailRepository, CallDetailRepository>();
        
        return services;
    }
}