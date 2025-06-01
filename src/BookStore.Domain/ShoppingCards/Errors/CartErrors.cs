using BookStore.SharedKernel.Abstractions.Errors;



namespace BookStore.Domain.ShoppingCards.Errors
{
    public static class CartErrors
    {
        public static readonly Error CartNotFound =
            new(
                "Cart.NotFound",
                "No active cart was found for the current user.",
                 ErrorType.NotFound
            );

        public static readonly Error BookNotFound =
            new(
                "Cart.BookNotFound",
                "The specified book does not exist or is unavailable.",
                ErrorType.NotFound
            );

        public static readonly Error InvalidQuantity =
            new(
                "Cart.InvalidQuantity",
                "Quantity must be at least 1.",
                ErrorType.Validation
            );

        public static readonly Error CartItemNotFound =
            new(
                "Cart.ItemNotFound",
                "The specified item was not found in the cart.",
                ErrorType.NotFound
            );

        public static readonly Error CartAlreadyCheckedOut =
            new(
                "Cart.AlreadyCheckedOut",
                "Cannot modify a cart that has already been checked out.",
                ErrorType.Conflict
            );

        public static readonly Error CartEmptyOnCheckout =
            new(
                "Cart.EmptyOnCheckout",
                "Cannot checkout because the cart is empty.",
                ErrorType.Validation
            );
    }
}
