using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Orders;

namespace BookStore.Application.Orders.Queries.ListOrders;

public sealed record ListOrdersQuery() : IQuery<IEnumerable<OrderResponse>>;