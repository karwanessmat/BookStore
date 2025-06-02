using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Orders;

namespace BookStore.Application.Orders.Commands.PlaceOrder;

public sealed record PlaceOrderCommand(CheckoutRequest Request) : ICommand<Guid>;


