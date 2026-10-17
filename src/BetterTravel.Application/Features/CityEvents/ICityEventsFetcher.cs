using BetterTravel.Application.Features.CityEvents.Dtos;

namespace BetterTravel.Application.Features.CityEvents;

public interface ICityEventsFetcher
{
    Task<IReadOnlyCollection<CityEventDto>> FetchAsync(EventFetchFilterDto filter);
}