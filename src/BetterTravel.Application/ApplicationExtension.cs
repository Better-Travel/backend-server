using BetterTravel.Application.Features.Cities.Events;
using BetterTravel.Application.Features.Cities.TouristRoutes;
using Microsoft.Extensions.DependencyInjection;

namespace BetterTravel.Application;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICityEventsFetcher, CityEventsFetcher>();
        services.AddScoped<ICityTouristRouteFetcher, CityTouristRouteFetcher>();
        
        return services;
    }
}