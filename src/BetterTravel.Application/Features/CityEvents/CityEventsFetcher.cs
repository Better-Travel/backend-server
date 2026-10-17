using BetterTravel.Application.Features.CityEvents.Dtos;

namespace BetterTravel.Application.Features.CityEvents;

public class CityEventsFetcher(IEnumerable<ICityClient> cityClients) : ICityEventsFetcher
{
    public async Task<IReadOnlyCollection<CityEventDto>> FetchAsync(EventFetchFilterDto filter)
    {
        var client = cityClients.Single(client => string.Equals(client.City, filter.City));

        return await client.GetEventsAsync();
    }
}