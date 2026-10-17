using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetterTravel.Infrastructure.Warsaw;

public static class WarsawInfrastructureExtension
{
    public static IServiceCollection AddWarsawInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddOptions<WarsawClientOptions>()
            .Bind(configuration.GetSection(WarsawClientOptions.Name))
            .ValidateDataAnnotations();
        
        services.AddScoped<WarsawClient>();
        
        return services;
    }
}