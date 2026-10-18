using BetterTravel.Entities.ValueObjects;

namespace BetterTravel.Entities;

public class TouristRoute
{
    public string? Title { get; init; }
    
    public string? Description { get; init; }
    
    public float Length { get; init; }
    
    public required IReadOnlyCollection<GeoCoordinate> Coordinate { get; init; }
}