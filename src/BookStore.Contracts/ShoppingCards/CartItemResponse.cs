namespace BookStore.Contracts.ShoppingCards;

public record CartItemResponse(
    Guid CartItemId,
    Guid BookId,
    string BookTitle,
    int Quantity,
    decimal UnitPrice,
    string Currency,
    decimal SubTotal);