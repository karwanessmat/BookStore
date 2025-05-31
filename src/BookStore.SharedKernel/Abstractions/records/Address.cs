

namespace BookStore.SharedKernel.Abstractions.records;

public class Address
{
    public string? UnitDetails { get; private set; }
    public string? Street { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }
    public Location? GeoLocation { get; private set; }

    // Parameterless constructor for EF Core
    private Address() { }

    public Address(string? unitDetails, string? street, string? city, string? state, string? country, Location? geoLocation)
    {
        UnitDetails = unitDetails ??"";
        Street = street ?? ""; ;
        City = city ?? ""; ;
        State = state ?? ""; ;
        Country = country ?? ""; ;
        GeoLocation = geoLocation ;
    }

    // Method to update Address properties without replacing GeoLocation instance
    public void Update(
        string? unitDetails,
        string? street,
        string? city,
        string? state,
        string? country,
        string? latitude,
        string? longitude)
    {
        UnitDetails = unitDetails ?? ""; ;
        Street = street ?? ""; ;
        City = city ?? ""; ;
        State = state ?? ""; ;
        Country = country ?? ""; ;

        if (GeoLocation != null)
        {
            // Update existing GeoLocation properties
            GeoLocation.Update(latitude, longitude);
        }
        else
        {
            // Create a new GeoLocation instance only if it doesn't exist
            GeoLocation = new Location(latitude, longitude);
        }
    }
}