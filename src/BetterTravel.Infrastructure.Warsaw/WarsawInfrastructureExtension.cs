using BetterTravel.Application.Features.Cities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BetterTravel.Infrastructure.Warsaw;

public static class WarsawInfrastructureExtension
{
    private const string BaseUrl = "https://dane.um.warszawa.pl/api/action";

    public static IServiceCollection AddWarsawInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddOptions<WarsawClientOptions>()
            .Bind(configuration.GetSection(WarsawClientOptions.Name))
            .ValidateDataAnnotations();

        services.AddHttpClient<ICityClient, WarsawClient>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(BaseUrl);
            var clientOptions = serviceProvider.GetRequiredService<IOptions<WarsawClientOptions>>();
            client.DefaultRequestHeaders.Add("Authorization", clientOptions.Value.Token);
        });
        
        return services;
    }
}