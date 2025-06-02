namespace BookStore.Contracts.Orders;

public record CheckoutRequest(
    string FullName,
    string Line1,
    string? Line2,
    string City,
    string State,
    string PostalCode,
    string Country,
    decimal ShippingCost );
