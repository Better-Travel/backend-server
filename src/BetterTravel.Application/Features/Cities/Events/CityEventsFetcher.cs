using BetterTravel.Application.Features.Cities.Events.Dtos;
using BetterTravel.Entities;
using BetterTravel.Entities.ValueObjects;

namespace BetterTravel.Application.Features.Cities.Events;

public class CityEventsFetcher(IEnumerable<ICityClient> cityClients) : ICityEventsFetcher
{
    public async Task<IReadOnlyCollection<CityEventDto>> FetchAsync(EventFetchFilterDto filter, CancellationToken cancellationToken)
    {
        var client = cityClients.Single(client => string.Equals(client.City, filter.City));

        var cityEvents = await client.GetEventsAsync(cancellationToken);
        
        if (filter.From is null || filter.To is null)
        {
            return cityEvents.Select(MapCityEvent).ToArray();
        }
        
        return cityEvents
            .Where(e => e.Occurrences.Any(o => IsEventInGivenDateRange(o, filter.From.Value, filter.To.Value)))
            .Select(MapCityEvent)
            .ToArray();
    }

    private static CityEventDto MapCityEvent(CityEvent cityEvent) =>
        new(
            cityEvent.Title,
            cityEvent.Description,
            cityEvent.Url?.ToString(),
            cityEvent.Coordinates.Select(c => new GeoCoordinateDto(c.Latitude, c.Longitude)).ToArray(),
            cityEvent.LocationNames,
            cityEvent.EventCategories,
            cityEvent.Addresses,
            cityEvent.Occurrences.Select(o => new OccurrenceDto(o.From, o.To)).ToArray(),
            cityEvent.IsRepeatable,
            cityEvent.Image is null ? null : new EventImageDto(cityEvent.Image.Highlighted, cityEvent.Image.Small));

    private static bool IsEventInGivenDateRange(Occurrence occurrence, DateTime from, DateTime to) =>
        from >= occurrence.From && from <= occurrence.To || occurrence.From <= to && occurrence.To >= from;
}