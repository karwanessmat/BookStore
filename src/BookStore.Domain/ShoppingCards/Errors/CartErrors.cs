using BookStore.SharedKernel.Abstractions.Errors;

namespace BookStore.Domain.ShoppingCards.Errors;

public static class CartErrors
{
    public static readonly Error NotFound =
        new(
            "Cart.NotFound",
            "The Cart with the specified identifier was not found",
            ErrorType.NotFound);
}