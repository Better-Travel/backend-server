using System.Net.Http.Json;
using BetterTravel.Application.Features.CityEvents;
using BetterTravel.Application.Features.CityEvents.Dtos;
using BetterTravel.Entities.ValueObjects;
using BetterTravel.Infrastructure.Warsaw.Models;
using BetterTravel.Infrastructure.Warsaw.ValueObjects;

namespace BetterTravel.Infrastructure.Warsaw;

internal class WarsawClient(
    WarsawClientOptions clientOptions,
    IHttpClientFactory httpClientFactory
    ) : ICityClient
{
    public string City => "Warsaw";

    private const string ClientName = "BetterTravel";
    
    private const string BaseUrl = "https://dane.um.warszawa.pl/api/action";

    public async Task<IReadOnlyCollection<CityEventDto>> GetEventsAsync()
    {
        const string endpointRequest = "/get_bmm_kalendarz_wydarzen";
        
        using var client = httpClientFactory.CreateClient(ClientName);
        
        client.BaseAddress = new Uri(BaseUrl);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {clientOptions.Token}");

        var response = await client.PostAsync(endpointRequest, null);
        var events = await response.Content.ReadFromJsonAsync<List<WarsawEvent>>();

        var filteredEvents = events.Where(e => e.Map is null);
        
        // TODO: Handle errors
        return [.. filteredEvents.Select(MapEvent)];
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
    
    private static OccurrenceDto MapOccurrences(Occurrence warsawEvent)
    {
        return new OccurrenceDto(warsawEvent.From, warsawEvent.To);
    }
    
    private static EventImageDto? MapImages(WarsawEvent warsawEvent) =>
        warsawEvent.Image == null ? 
            null : 
            new EventImageDto(new Uri(warsawEvent.Image.Highlighted.Path), new Uri(warsawEvent.Image.Small.Path));
}
