namespace BetterTravel.Application.Features.Cities.Events.Dtos;

public record EventFetchFilterDto(DateTime? From, DateTime? To, string City);