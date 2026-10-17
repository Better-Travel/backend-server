using BetterTravel.Application.Features.CityEvents.Dtos;

namespace BetterTravel.Application.Features.CityEvents;

public class CityEventsFetcher(IEnumerable<ICityClient> cityClients) : ICityEventsFetcher
{
    public async Task<IReadOnlyCollection<CityEventDto>> FetchAsync(EventFetchFilterDto filter)
    {
        var client = cityClients.Single(client => string.Equals(client.City, filter.City));

        var cityEvents = await client.GetEventsAsync();
        
        if (filter.From is null || filter.To is null)
        {
            return cityEvents;
        }
        
         return cityEvents
             .Where(e => 
                 e.Occurrences.Any(o => IsEventInGivenDateRange(o, filter.From.Value, filter.To.Value)))
             .ToArray();
    }

    private static bool IsEventInGivenDateRange(OccurrenceDto occurence, DateTime From, DateTime To)
    {
        if (From >= occurence.From && From <= occurence.To)
        {
            return true;
        }
        
        return occurence.From <= To && occurence.To >= From;
    }
}