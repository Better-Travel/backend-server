using BetterTravel.Application.Features.CityEvents;
using Microsoft.Extensions.DependencyInjection;

namespace BetterTravel.Application;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICityEventsFetcher, CityEventsFetcher>();
        
        return services;
    }
}