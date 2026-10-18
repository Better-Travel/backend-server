using BetterTravel.Application.Features.Cities.Events.Dtos;

namespace BetterTravel.Application.Features.Cities.Events;

public interface ICityEventsFetcher
{
    Task<IReadOnlyCollection<CityEventDto>> FetchAsync(EventFetchFilterDto filter);
}