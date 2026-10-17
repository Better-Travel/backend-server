namespace BetterTravel.Infrastructure.Warsaw.ValueObjects;

internal record Address
{
    public string? Name { get; init; }
    public string? Street { get; init; }
    public string? PostCodeAndOffice { get; init; }
}