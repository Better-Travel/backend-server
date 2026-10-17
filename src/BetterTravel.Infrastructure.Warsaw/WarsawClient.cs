using System.Net.Http.Json;
using BetterTravel.Application.Features.CityEvents;
using BetterTravel.Infrastructure.Warsaw.Models;

namespace BetterTravel.Infrastructure.Warsaw;

internal class WarsawClient(
    WarsawClientOptions clientOptions,
    IHttpClientFactory httpClientFactory
    ) : ICityClient
{
    public string City => "Warsaw";

    private const string ClientName = "BetterTravel";
    
    private const string BaseUrl = "https://dane.um.warszawa.pl/api/action";

    public async Task<CityEventDto> GetEventsAsync()
    {
        const string endpointRequest = "/get_bmm_kalendarz_wydarzen";
        
        using var client = httpClientFactory.CreateClient(ClientName);
        
        client.BaseAddress = new Uri(BaseUrl);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {clientOptions.Token}");

        //TODO: Use POST
        var @event = await client.GetFromJsonAsync<WarsawEvent>(endpointRequest);
        
        throw new NotImplementedException();
    }
}