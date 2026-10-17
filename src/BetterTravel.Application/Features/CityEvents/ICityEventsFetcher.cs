namespace BetterTravel.Application.Features.CityEvents;

public interface ICityEventsFetcher
{
    Task FetchAsync(EventFetchFilterDto? filter);
}