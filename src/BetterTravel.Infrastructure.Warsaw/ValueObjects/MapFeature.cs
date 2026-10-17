using System.Text.Json.Serialization;
using BetterTravel.Infrastructure.Warsaw.ValueObjects.MapFeatures;

namespace BetterTravel.Infrastructure.Warsaw.ValueObjects;

internal record MapFeature(Geometry Geometry, [property: JsonPropertyName("properties")] Property? Property, string Type);
