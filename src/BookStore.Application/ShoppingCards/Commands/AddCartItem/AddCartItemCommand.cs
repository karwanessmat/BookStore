using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.ShoppingCards;

namespace BookStore.Application.ShoppingCards.Commands.AddCartItem;

public sealed record AddCartItemCommand( AddCartItemRequest Request) : ICommand<Guid>;