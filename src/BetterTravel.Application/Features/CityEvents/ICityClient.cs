namespace BetterTravel.Application.Features.CityEvents;

public interface ICityClient
{
    string City { get; }

    Task GetEventsAsync();
}