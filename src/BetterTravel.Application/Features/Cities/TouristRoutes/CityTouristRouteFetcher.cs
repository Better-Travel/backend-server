using BetterTravel.Application.Features.Cities.Events.Dtos;
using BetterTravel.Application.Features.Cities.TouristRoutes.Dtos;

namespace BetterTravel.Application.Features.Cities.TouristRoutes;

internal class CityTouristRouteFetcher(IEnumerable<ICityClient> cityClients) : ICityTouristRouteFetcher
{
    public async Task<IReadOnlyCollection<TouristRouteDto>> FetchAsync(TouristRouteRequestDto request, CancellationToken cancellationToken)
    {
        var client = cityClients.Single(c => string.Equals(c.City, request.City));

        var routes = await client.GetTouristRoutesAsync(cancellationToken);

        return routes.Select(r
            => new TouristRouteDto(
                r.Title,
                r.Description,
                r.Length,
                r.Coordinate.Select(c => new GeoCoordinateDto(c.Latitude, c.Longitude)).ToArray()
            )
        ).ToArray();
    }
}