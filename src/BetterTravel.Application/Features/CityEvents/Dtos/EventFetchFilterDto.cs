namespace BetterTravel.Application.Features.CityEvents.Dtos;

public record EventFetchFilterDto(DateTime? From, DateTime? To, string City);