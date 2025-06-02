using BookStore.SharedKernel.Abstractions.Errors;

namespace BookStore.Domain.Orders.Errors
{
    public static class OrderErrors
    {
        public static readonly Error OrderNotFound =
            new(
                "Order.NotFound",
                "The specified order does not exist.",
                ErrorType.NotFound
            );

        public static readonly Error OrderItemNotFound =
            new(
                "Order.ItemNotFound",
                "The specified item was not found in the order.",
                ErrorType.NotFound
            );

        public static readonly Error InvalidOrderStatus =
            new(
                "Order.InvalidStatus",
                "The order status is invalid for this operation.",
                ErrorType.Validation
            );

        public static readonly Error OrderEmptyOnProcess =
            new(
                "Order.EmptyOnProcess",
                "Cannot process an order with no items.",
                ErrorType.Validation
            );

        public static readonly Error OrderAlreadyProcessed =
            new(
                "Order.AlreadyProcessed",
                "The order has already been processed and cannot be modified.",
                ErrorType.Conflict
            );

        public static readonly Error PaymentFailed =
            new(
                "Order.PaymentFailed",
                "Payment processing failed for the order.",
                ErrorType.Validation
            );
    }
}