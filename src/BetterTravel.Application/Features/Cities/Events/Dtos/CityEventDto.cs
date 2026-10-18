namespace BetterTravel.Application.Features.Cities.Events.Dtos;

public record CityEventDto(
    string Title, 
    string? Description, 
    string Url,
    GeoCoordinateDto[] Locations,
    string[] LocationNames,
    string[] EventCategories,
    string[] Addresses,
    OccurrenceDto[] Occurrences,
    bool IsRepeatable,
    EventImageDto? Image
    );