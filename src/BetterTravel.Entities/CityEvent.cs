namespace BetterTravel.Entities;

public class CityEvent
{
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public Uri Url { get; set; }
    
    public long Latitude { get; set; }
    
    public long Longitude { get; set; }
    
    public string LocationName { get; set; }
    
    public string? EventType { get; set; }
    
    public string? Address { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsRepeatable { get; set; }
}