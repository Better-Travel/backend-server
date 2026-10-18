using System.Net.Http.Json;
using BetterTravel.Application.Features.Cities;
using BetterTravel.Application.Features.Cities.Events.Dtos;
using BetterTravel.Entities;
using BetterTravel.Entities.ValueObjects;
using BetterTravel.Infrastructure.Warsaw.Models;
using BetterTravel.Infrastructure.Warsaw.ValueObjects;

namespace BetterTravel.Infrastructure.Warsaw;

internal class WarsawClient(
    HttpClient client
    ) : ICityClient
{
    public string City => "Warsaw";

    public async Task<IReadOnlyCollection<CityEventDto>> GetEventsAsync()
    {
        const string endpointRequest = "action/get_bmm_kalendarz_wydarzen";

        var response = await client.PostAsync(endpointRequest, null);
        var events = await response.Content.ReadFromJsonAsync<List<WarsawEvent>>();

        if (response.IsSuccessStatusCode == false || events is null)
        {
            throw new Exception("Failed to fetch events");
        }

        var filteredEvents = events.Where(e => e.Map is null);
        
        // TODO: Handle errors
        return [.. filteredEvents.Select(MapEvent)];
    }

    public async Task<IReadOnlyCollection<TouristRoute>> GetTouristRoutesAsync()
    {
        const string endpointRequest = "action/get_sbt_trasy_turystyczne";

        var response = await client.PostAsync(endpointRequest, null);

        if (response.IsSuccessStatusCode == false)
        {
            throw new Exception("Failed to fetch tourist routes");
        }

        var routeResponse = await response.Content.ReadFromJsonAsync<WarsawRoutesResponse>();
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

    private static CityEventDto MapEvent(WarsawEvent warsawEvent)
    {
        IEnumerable<GeoCoordinateDto> locations;

        if (warsawEvent.Map is null || warsawEvent.Map.Features.Count == 0)
        {
            locations = [];
        }
        else
        {
            locations = warsawEvent.Map.Features.Select(f => MapCoordinate(f.Geometry.Coordinates));   
        }

        return new CityEventDto(
            warsawEvent.Title,
            warsawEvent.Description,
            warsawEvent.Url.ToString(),
            [.. locations],
            MapLocationNames(warsawEvent),
            [.. warsawEvent.Categories.Select(c => c.Name)],
            MapAddresses(warsawEvent),
            [.. warsawEvent.Occurrences.Select(MapOccurrences)],
            warsawEvent.Occurrences.Any(o => o.Daily),
            MapImages(warsawEvent)
        );
    }

    private static string[] MapLocationNames(WarsawEvent warsawEvent) =>
    [
        .. warsawEvent.Addresses
            .Select(a => a.Name?.Trim())
            .Where(n => !string.IsNullOrEmpty(n))
            .Distinct()
            .Select(n => n!)
    ];

    private static string[] MapAddresses(WarsawEvent warsawEvent)
    {
        var addresses = warsawEvent.Addresses
            .Select(a => JoinNonEmpty(", ", a.Street, a.PostCodeAndOffice))
            .Where(a => a.Length > 0)
            .Distinct()
            .ToArray();

        if (addresses.Length > 0)
        {
            return addresses;
        }

        return (warsawEvent.Map?.Features ?? [])
            .Where(f => f.Property != null)
            .Select(f => JoinNonEmpty(
                ", ",
                JoinNonEmpty(" ", f.Property!.ShortStreetName, f.Property.StreetNumber),
                f.Property.District))
            .Where(a => a.Length > 0)
            .Distinct()
            .ToArray();
    }

    private static string JoinNonEmpty(string separator, params string?[] parts) =>
        string.Join(separator, parts.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p!.Trim()));

    private static GeoCoordinateDto MapCoordinate(IReadOnlyCollection<double> coordinates)
    {
        const int latitudeIndex = 0;
        const int longitudeIndex = 1;

        return new GeoCoordinateDto(coordinates.ElementAt(latitudeIndex), coordinates.ElementAt(longitudeIndex));
    }
    
    private static GeoCoordinate MapCoordinat(IReadOnlyCollection<double> coordinates)
    {
        const int latitudeIndex = 0;
        const int longitudeIndex = 1;

        return new GeoCoordinate(coordinates.ElementAt(latitudeIndex), coordinates.ElementAt(longitudeIndex));
    }
    
    private static OccurrenceDto MapOccurrences(Occurrence warsawEvent)
    {
        return new OccurrenceDto(warsawEvent.From, warsawEvent.To);
    }
    
    private static EventImageDto? MapImages(WarsawEvent warsawEvent)
    {
        if (warsawEvent.Image is null)
        {
            return null;
        }

        Uri? highlightedImageUri = null;

        if (warsawEvent.Image.Highlighted?.Path is not null)
        {
            highlightedImageUri = new Uri(warsawEvent.Image.Highlighted.Path);
        }

        Uri? smallImageUri = null;

        if (warsawEvent.Image.Small?.Path is not null)
        {
            smallImageUri = new Uri(warsawEvent.Image.Small.Path);
        }

        return warsawEvent.Image == null
            ? null
            : new EventImageDto(highlightedImageUri, smallImageUri);
    }

    private sealed record WarsawRoutesResponse(IReadOnlyCollection<WarsawRoute>? Result);
}
