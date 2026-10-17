using BetterTravel.Entities.ValueObjects;

namespace BetterTravel.Entities;

public class CityEvent
{
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public Uri? Url { get; set; }

    public IList<GeoCoordinate> Coordinates { get; set; } = [];
    
    //public string LocationName { get; set; }
    
    public string? EventType { get; set; }
    
    public string? Address { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsRepeatable { get; set; }
    
    public Uri[] Images { get; set; } = [];
}