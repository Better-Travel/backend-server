using BetterTravel.Application.Features.CityEvents;
using BetterTravel.Application.Features.CityEvents.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BetterTravel.API.Controllers;

[ApiController]
public class WarsawLookupController(ICityEventsFetcher eventsFetcher) : ControllerBase
{
    [HttpGet("/api/better-travel/warsaw/events")]
    public async Task<ActionResult<IReadOnlyCollection<CityEventDto>>> GetEvents([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var events = await eventsFetcher.FetchAsync(new EventFetchFilterDto(from, to, "Warsaw"));

        return Ok(events);
    }
}