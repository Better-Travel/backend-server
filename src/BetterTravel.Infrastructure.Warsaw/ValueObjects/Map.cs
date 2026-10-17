namespace BetterTravel.Infrastructure.Warsaw.ValueObjects;

internal record Map(IReadOnlyCollection<MapFeature> Features, string Type);