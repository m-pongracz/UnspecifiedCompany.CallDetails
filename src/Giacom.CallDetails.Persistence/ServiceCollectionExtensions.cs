using Giacom.CallDetails.Persistence.CallDetails;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Giacom.CallDetails.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO EF
        
        services.AddScoped<ICallDetailRepository, CallDetailRepository>();
        
        return services;
    }
}