using Giacom.CallDetails.Application.CallDetails;
using Microsoft.Extensions.DependencyInjection;

namespace Giacom.CallDetails.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICallDetailService, CallDetailService>();

        return services;
    }
}