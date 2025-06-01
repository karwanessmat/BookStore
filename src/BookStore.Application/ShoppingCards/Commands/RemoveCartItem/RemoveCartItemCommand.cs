using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.ShoppingCards;

namespace BookStore.Application.ShoppingCards.Commands.RemoveCartItem;

public sealed record RemoveCartItemCommand(RemoveCartItemRequest Request) : ICommand;