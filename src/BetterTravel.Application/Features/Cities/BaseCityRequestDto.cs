namespace BetterTravel.Application.Features.Cities;

public abstract record BaseCityRequestDto
{
    public required string City { get; init; }   
}