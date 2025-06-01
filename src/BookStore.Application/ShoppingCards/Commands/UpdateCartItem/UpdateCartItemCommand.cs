using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.ShoppingCards;
using BookStore.Domain.ShoppingCards;

namespace BookStore.Application.ShoppingCards.Commands.UpdateCartItem;

public sealed record UpdateCartItemCommand(UpdateCartItemRequest Request) : ICommand;