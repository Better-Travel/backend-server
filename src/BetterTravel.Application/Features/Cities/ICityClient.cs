using BetterTravel.Application.Features.Cities.Events.Dtos;
using BetterTravel.Entities;

namespace BetterTravel.Application.Features.Cities;

public interface ICityClient
{
    string City { get; }

    Task<IReadOnlyCollection<CityEventDto>> GetEventsAsync();

    Task<IReadOnlyCollection<TouristRoute>> GetTouristRoutesAsync();
}