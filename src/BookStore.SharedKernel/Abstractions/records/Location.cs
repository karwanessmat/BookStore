namespace BookStore.SharedKernel.Abstractions.records;

public class Location
{
    public string? Latitude { get; private set; }
    public string? Longitude { get; private set; }

    // Parameterless constructor for EF Core
    private Location() { }

    // Constructor to initialize Location
    public Location(string? latitude, string? longitude)
    {
        Latitude = latitude ?? ""; ;
        Longitude = longitude ?? ""; ;
    }

    // Method to update Location properties
    public void Update(string? latitude, string? longitude)
    {
        Latitude = latitude ?? ""; ;
        Longitude = longitude ?? ""; ;
    }
}