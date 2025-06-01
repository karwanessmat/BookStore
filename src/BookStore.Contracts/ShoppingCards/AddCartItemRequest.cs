namespace BookStore.Contracts.ShoppingCards;

public record AddCartItemRequest(Guid BookId, int Quantity = 1);