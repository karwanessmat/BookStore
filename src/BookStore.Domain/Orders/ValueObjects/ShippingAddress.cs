using BookStore.SharedKernel.Abstractions;


namespace BookStore.Domain.Orders.ValueObjects;

public sealed class ShippingAddress : ValueObject
{
    private ShippingAddress() { }        

    public string FullName { get; private set; } = default!;
    public string Line1 { get; private set; } = default!;
    public string? Line2 { get; private set; }
    public string City { get; private set; } = default!;
    public string State { get; private set; } = default!;
    public string PostalCode { get; private set; } = default!;
    public string Country { get; private set; } = default!;

    private ShippingAddress(
        string fullName,
        string line1,
        string? line2,
        string city,
        string state,
        string postalCode,
        string country)
    {
        FullName = fullName;
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
    }

    public static ShippingAddress Create(string fullName, string line1, string? line2,string city,string state,string postalCode, string country)
        => new(fullName, line1, line2, city, state, postalCode, country);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FullName;
        yield return Line1;
        yield return Line2;
        yield return City;
        yield return State;
        yield return PostalCode;
        yield return Country;
    }
}
