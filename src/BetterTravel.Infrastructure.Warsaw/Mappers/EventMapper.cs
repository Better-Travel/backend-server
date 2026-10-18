using BetterTravel.Entities;
using BetterTravel.Entities.ValueObjects;
using BetterTravel.Infrastructure.Warsaw.Models;
using CityEventImage = BetterTravel.Entities.ValueObjects.EventImage;
using CityEventOccurrence = BetterTravel.Entities.ValueObjects.Occurrence;
using WarsawOccurrence = BetterTravel.Infrastructure.Warsaw.ValueObjects.Occurrence;

namespace BetterTravel.Infrastructure.Warsaw.Mappers;

internal static class EventMapper
{
    public static CityEvent Map(WarsawEvent warsawEvent)
    {
        IEnumerable<GeoCoordinate> locations;

        if (warsawEvent.Map is null || warsawEvent.Map.Features.Count == 0)
        {
            locations = [];
        }
        else
        {
            locations = warsawEvent.Map.Features.Select(f => MapCoordinate(f.Geometry.Coordinates));   
        }

        return new CityEvent
        {
            Title = warsawEvent.Title,
            Description = warsawEvent.Description,
            Url = warsawEvent.Url,
            Coordinates = [.. locations],
            LocationNames = MapLocationNames(warsawEvent),
            EventCategories = [.. warsawEvent.Categories.Select(c => c.Name)],
            Addresses = MapAddresses(warsawEvent),
            Occurrences = [.. warsawEvent.Occurrences.Select(MapOccurrence)],
            IsRepeatable = warsawEvent.Occurrences.Any(o => o.Daily),
            Image = MapImages(warsawEvent)
        };
    }

    private static string[] MapLocationNames(WarsawEvent warsawEvent) =>
    [
        .. warsawEvent.Addresses
            .Select(a => a.Name?.Trim())
            .Where(n => !string.IsNullOrEmpty(n))
            .Distinct()
            .Select(n => n!)
    ];

    private static string[] MapAddresses(WarsawEvent warsawEvent)
    {
        var addresses = warsawEvent.Addresses
            .Select(a => JoinNonEmpty(", ", a.Street, a.PostCodeAndOffice))
            .Where(a => a.Length > 0)
            .Distinct()
            .ToArray();

        if (addresses.Length > 0)
        {
            return addresses;
        }

        return (warsawEvent.Map?.Features ?? [])
            .Where(f => f.Property != null)
            .Select(f => JoinNonEmpty(
                ", ",
                JoinNonEmpty(" ", f.Property!.ShortStreetName, f.Property.StreetNumber),
                f.Property.District))
            .Where(a => a.Length > 0)
            .Distinct()
            .ToArray();
    }

    private static string JoinNonEmpty(string separator, params string?[] parts) =>
        string.Join(separator, parts.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p!.Trim()));

    private static GeoCoordinate MapCoordinate(IReadOnlyCollection<double> coordinates)
    {
        const int latitudeIndex = 0;
        const int longitudeIndex = 1;

        return new GeoCoordinate(coordinates.ElementAt(latitudeIndex), coordinates.ElementAt(longitudeIndex));
    }
    
    private static CityEventOccurrence MapOccurrence(WarsawOccurrence warsawEvent) =>
        new() { From = warsawEvent.From, To = warsawEvent.To };
    
    private static CityEventImage? MapImages(WarsawEvent warsawEvent)
    {
        if (warsawEvent.Image is null)
        {
            return null;
        }

        Uri? highlightedImageUri = null;

        if (warsawEvent.Image.Highlighted?.Path is not null)
        {
            highlightedImageUri = new Uri(warsawEvent.Image.Highlighted.Path);
        }

        Uri? smallImageUri = null;

        if (warsawEvent.Image.Small?.Path is not null)
        {
            smallImageUri = new Uri(warsawEvent.Image.Small.Path);
        }

        return new CityEventImage
        {
            Highlighted = highlightedImageUri,
            Small = smallImageUri
        };
    }
}