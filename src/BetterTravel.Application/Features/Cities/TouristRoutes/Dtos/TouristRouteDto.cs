using BetterTravel.Application.Features.Cities.Events.Dtos;

namespace BetterTravel.Application.Features.Cities.TouristRoutes.Dtos;

public record TouristRouteDto(string? Title, string? Description, float Length, GeoCoordinateDto[] Locations);