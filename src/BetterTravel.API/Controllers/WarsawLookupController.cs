using BetterTravel.Application.Features.Cities.Events;
using BetterTravel.Application.Features.Cities.Events.Dtos;
using BetterTravel.Application.Features.Cities.TouristRoutes;
using BetterTravel.Application.Features.Cities.TouristRoutes.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BetterTravel.API.Controllers;

[ApiController]
public class WarsawLookupController(
    ICityEventsFetcher eventsFetcher,
    ICityTouristRouteFetcher touristRouteFetcher
    ) : ControllerBase
{
    [HttpGet("/api/better-travel/warsaw/events")]
    public async Task<ActionResult<IReadOnlyCollection<CityEventDto>>> GetEvents([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var events = await eventsFetcher.FetchAsync(new EventFetchFilterDto(from, to, "Warsaw"));

        return Ok(events);
    }
    
    [HttpGet("/api/better-travel/warsaw/tourist-routes")]
    public async Task<ActionResult<IReadOnlyCollection<TouristRouteDto>>> GetTouristRoutes()
    {
        var routes = await touristRouteFetcher.FetchAsync(new TouristRouteRequestDto
        {
            City = "Warsaw",
        });
        
        return Ok(routes);
    }
}