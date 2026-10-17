namespace BetterTravel.Application.Features.CityEvents.Dtos;

public record CityEventDto(
    string Title, 
    string Description, 
    string Url,
    GeoCoordinateDto[] Locations,
    string LocationName,
    string[] EventCategories,
    string? Address,
    OccurrencesDto[] Occurrences,
    bool IsRepeatable,
    EventImageDto? Image
    );