namespace BetterTravel.Entities.ValueObjects;

public record EventImage
{
    public Uri? Highlighted { get; init; }
    
    public Uri? Small { get; init; }
}