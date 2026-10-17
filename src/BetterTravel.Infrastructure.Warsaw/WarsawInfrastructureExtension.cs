using BetterTravel.Application.Features.CityEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BetterTravel.Infrastructure.Warsaw;

public static class WarsawInfrastructureExtension
{
    public static IServiceCollection AddWarsawInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddHttpClient();
        
        services.AddOptions<WarsawClientOptions>()
            .Bind(configuration.GetSection(WarsawClientOptions.Name))
            .ValidateDataAnnotations();
        
        services.AddScoped<ICityClient, WarsawClient>();
        
        return services;
    }
}