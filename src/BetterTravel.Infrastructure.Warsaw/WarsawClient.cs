using System.Net.Http.Json;
using BetterTravel.Application.Features.Cities;
using BetterTravel.Entities;
using BetterTravel.Entities.ValueObjects;
using BetterTravel.Infrastructure.Warsaw.Mappers;
using BetterTravel.Infrastructure.Warsaw.Models;

namespace BetterTravel.Infrastructure.Warsaw;

internal class WarsawClient(
    HttpClient client
    ) : ICityClient
{
    public string City => "Warsaw";

    public async Task<IReadOnlyCollection<CityEvent>> GetEventsAsync(CancellationToken cancellationToken)
    {
        const string endpointRequest = "action/get_bmm_kalendarz_wydarzen";

        var response = await client.PostAsync(endpointRequest, null, cancellationToken);
        var events = await response.Content.ReadFromJsonAsync<List<WarsawEvent>>(cancellationToken);

        if (response.IsSuccessStatusCode == false || events is null)
        {
            throw new Exception("Failed to fetch events");
        }

        var filteredEvents = events.Where(e => e.Map is null);
        
        // TODO: Handle errors
        return [.. filteredEvents.Select(EventMapper.Map)];
    }

    public async Task<IReadOnlyCollection<TouristRoute>> GetTouristRoutesAsync(CancellationToken cancellationToken)
    {
        const string endpointRequest = "action/get_sbt_trasy_turystyczne";

        var response = await client.PostAsync(endpointRequest, null, cancellationToken);

        if (response.IsSuccessStatusCode == false)
        {
            throw new Exception("Failed to fetch tourist routes");
        }

        var routeResponse = await response.Content.ReadFromJsonAsync<WarsawRoutesResponse>(cancellationToken);
        var routes = routeResponse?.Result;

        if (routes is null)
        {
            throw new Exception("Failed to parse tourist routes");
        }

        return routes.Select(r => new TouristRoute
        {
            Title = r.Title,
            Description = r.Description,
            Length = r.Length,
            Coordinate = r.Traces.Select(MapCoordinat).ToList()
        }).ToArray();
    }
    
    private static GeoCoordinate MapCoordinat(IReadOnlyCollection<double> coordinates)
    {
        const int latitudeIndex = 0;
        const int longitudeIndex = 1;

        return new GeoCoordinate(coordinates.ElementAt(latitudeIndex), coordinates.ElementAt(longitudeIndex));
    }

    private sealed record WarsawRoutesResponse(IReadOnlyCollection<WarsawRoute>? Result);
}
