namespace BookStore.Contracts.ShoppingCards;

public record UpdateCartItemRequest(Guid CartItemId, int Quantity);