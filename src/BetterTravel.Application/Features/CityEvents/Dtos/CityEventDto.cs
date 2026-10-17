namespace BetterTravel.Application.Features.CityEvents.Dtos;

public record CityEventDto(
    string Title, 
    string Description, 
    string Url,
    GeoCoordinateDto[] Locations,
    string[] LocationNames,
    string[] EventCategories,
    string[] Addresses,
    OccurrenceDto[] Occurrences,
    bool IsRepeatable,
    EventImageDto? Image
    );