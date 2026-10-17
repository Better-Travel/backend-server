using Microsoft.AspNetCore.Mvc;

namespace BetterTravel.API.Controllers;

[ApiController]
public class MapLookupController : ControllerBase
{
    [HttpGet("/api/better-travel/map/places")]
    public IActionResult GetPlaces([FromBody] string query)
    {
        return Ok();
    }
}