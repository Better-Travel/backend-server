using BetterTravel.Application.Features.Cities.TouristRoutes.Dtos;

namespace BetterTravel.Application.Features.Cities.TouristRoutes;

public interface ICityTouristRouteFetcher
{
    Task<IReadOnlyCollection<TouristRouteDto>> FetchAsync(TouristRouteRequestDto request);
}