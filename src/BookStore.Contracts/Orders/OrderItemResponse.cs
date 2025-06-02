namespace BookStore.Contracts.Orders;

public record OrderItemResponse(
    Guid OrderItemId,
    Guid BookId,
    string BookTitle,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal);