namespace BetterTravel.Infrastructure.Warsaw.ValueObjects.MapFeatures;

internal record Geometry(IReadOnlyCollection<double> Coordinates, string Type);