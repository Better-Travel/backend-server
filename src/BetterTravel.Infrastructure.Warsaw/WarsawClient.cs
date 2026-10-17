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
        var events = await response.Content.ReadFromJsonAsync<WarsawEvent>();
        
        throw new NotImplementedException();
    }

    private static CityEventDto MapEvent(WarsawEvent warsawEvent)
    {
        return new CityEventDto(
            warsawEvent.Title,
            warsawEvent.Description,
            warsawEvent.Url.ToString(),
            warsawEvent.Map!.Features.Select(f => MapCoordinate(f.Geometry.Coordinates)).ToArray(),
             "",
             [.. warsawEvent.Categories.Select(c => c.Name)],
            "",
            [.. warsawEvent.Occurrences.Select(MapOccurrences)],
            warsawEvent.Occurrences.Any(o => o.Daily),
            MapImages(warsawEvent)
        );
    }

    private static GeoCoordinateDto MapCoordinate(IReadOnlyCollection<double> coordinates)
    {
        const int latitudeIndex = 0;
        const int longitudeIndex = 1;

        return new GeoCoordinateDto(coordinates.ElementAt(latitudeIndex), coordinates.ElementAt(longitudeIndex));
    }
    
    private static OccurrencesDto MapOccurrences(Occurrence warsawEvent)
    {
        return new OccurrencesDto(warsawEvent.From, warsawEvent.To);
    }
    
    private static EventImageDto? MapImages(WarsawEvent warsawEvent) =>
        warsawEvent.Image == null ? 
            null : 
            new EventImageDto(new Uri(warsawEvent.Image.Highlighted.Path), new Uri(warsawEvent.Image.Small.Path));
}