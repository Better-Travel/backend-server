using Microsoft.AspNetCore.Mvc;

namespace BetterTravel.API.Controllers;

[ApiController]
public class WarsawLookupController : ControllerBase
{
    [HttpGet("/api/better-travel/warsaw/events")]
    public IActionResult GetEvents([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        // https://dane.um.warszawa.pl/pl/catalogue/ba202e4b-dfa9-440c-83c0-0b3517520cb8
        return Ok();
    }
}