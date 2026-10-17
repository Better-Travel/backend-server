namespace BetterTravel.Infrastructure.Warsaw.ValueObjects;

internal record Address
{
    public required string Name { get; init; }
    public string? Street { get; init; }
}