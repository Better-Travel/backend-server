namespace BetterTravel.Entities.ValueObjects;

public record Occurrence
{
    public DateTime From { get; init; }
    
    public DateTime To { get; init; }
}