using BetterTravel.Application.Features.CityEvents.Dtos;
using BetterTravel.Entities;

namespace BetterTravel.Application.Features.CityEvents;

public interface ICityClient
{
    string City { get; }

    Task<IReadOnlyCollection<CityEventDto>> GetEventsAsync();
}