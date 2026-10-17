using BetterTravel.Infrastructure.Warsaw.ValueObjects.MapFeatures;

namespace BetterTravel.Infrastructure.Warsaw.ValueObjects;

internal record MapFeature(Geometry Geometry, Property Property, string Type);