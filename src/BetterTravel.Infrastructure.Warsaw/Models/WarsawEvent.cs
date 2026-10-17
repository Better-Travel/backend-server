using System.Text.Json.Serialization;
using BetterTravel.Infrastructure.Warsaw.ValueObjects;

namespace BetterTravel.Infrastructure.Warsaw.Models;

internal record WarsawEvent
{
    public required string Title { get; init; }
    
    public required string Description { get; init; }
    
    public required Address Address { get; init; }

    [JsonPropertyName("occurrence")] 
    public IReadOnlyCollection<Occurrence> Occurrences { get; init; } = [];
    
    public required string Language { get; init; }

    public IReadOnlyCollection<string> AvailableLanguages { get; init; } = [];
    
    public required Uri Url { get; init; }
    
    public required Map Map { get; init; }
}