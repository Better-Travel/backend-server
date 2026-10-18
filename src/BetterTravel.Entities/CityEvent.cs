using BetterTravel.Entities.ValueObjects;

namespace BetterTravel.Entities;

public class CityEvent
{
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public Uri? Url { get; set; }

    public IList<GeoCoordinate> Coordinates { get; set; } = [];

    public string[] LocationNames { get; set; } = [];

    public string[] EventCategories { get; set; } = [];

    public string[] Addresses { get; set; } = [];
    
    public IList<Occurrence> Occurrences { get; set; } = [];
    
    public bool IsRepeatable { get; set; }

    public EventImage? Image { get; set; }
}