using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.ShoppingCards;

namespace BookStore.Application.ShoppingCards.Queries.GetCart;

public sealed record GetCartQuery() : IQuery<CartResponse>;