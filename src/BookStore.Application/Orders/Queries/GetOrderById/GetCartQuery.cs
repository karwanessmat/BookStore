using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Orders;

namespace BookStore.Application.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderResponse>;