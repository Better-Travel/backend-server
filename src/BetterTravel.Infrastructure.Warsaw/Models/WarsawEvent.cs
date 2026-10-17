using System.Text.Json.Serialization;
using BetterTravel.Infrastructure.Warsaw.ValueObjects;

namespace BetterTravel.Infrastructure.Warsaw.Models;

internal record WarsawEvent
{
    public required string Title { get; init; }
    
    public required string Description { get; init; }
    
    [JsonPropertyName("address")]
    public IReadOnlyCollection<Address> Addresses { get; init; } = [];

    [JsonPropertyName("occurrence")] 
    public IReadOnlyCollection<Occurrence> Occurrences { get; init; } = [];
    
    public required string Language { get; init; }

    public IReadOnlyCollection<string> AvailableLanguages { get; init; } = [];
    
    public required Uri Url { get; init; }
    
    public Map? Map { get; init; }
    
    [JsonPropertyName("category")] 
    public IReadOnlyCollection<Category> Categories { get; init; } = [];
    
    public EventImage? Image { get; init; }
}