namespace BookStore.Contracts.ShoppingCards;

public record CartResponse(
    Guid CartId,
    Guid UserId,
    DateTimeOffset CreatedDate,
    IEnumerable<CartItemResponse> Items,
    decimal Total,
    string Currency);