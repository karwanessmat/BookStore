namespace BookStore.Contracts.Orders;



public record OrderResponse(
    Guid OrderId,
    Guid UserId,
    DateTimeOffset OrderedDate,
    string Status,
    AddressResponse ShippingAddress,
    decimal ShippingCost,
    IEnumerable<OrderItemResponse> Items,
    decimal Total);

public record AddressResponse(
    string FullName,
    string Line1,
    string? Line2,
    string City,
    string State,
    string PostalCode,
    string Country);
