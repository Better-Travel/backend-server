namespace BetterTravel.Application.Features.CityEvents;

public record CityEventDto(
    string Title, 
    string Description, 
    string Url,
    double Latitude, 
    double Longitude,
    string LocationName,
    string? EventType,
    string? Address,
    DateTime StartDate,
    DateTime EndDate,
    bool IsRepeatable
    );