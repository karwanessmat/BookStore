using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.ShoppingCards;

namespace BookStore.Application.ShoppingCards.Commands.UpdateCartItem;

public sealed record UpdateCartItemCommand(UpdateCartItemRequest Request) : ICommand;