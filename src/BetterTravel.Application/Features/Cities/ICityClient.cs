using BetterTravel.Entities;

namespace BetterTravel.Application.Features.Cities;

public interface ICityClient
{
    string City { get; }

    Task<IReadOnlyCollection<CityEvent>> GetEventsAsync();

    Task<IReadOnlyCollection<TouristRoute>> GetTouristRoutesAsync();
}