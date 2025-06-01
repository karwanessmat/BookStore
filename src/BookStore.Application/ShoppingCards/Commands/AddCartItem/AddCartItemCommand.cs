using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.ShoppingCards;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.ShoppingCards;
using BookStore.Domain.ShoppingCards.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.ShoppingCards.Commands.AddCartItem;

public sealed record AddCartItemCommand( AddCartItemRequest Request) : ICommand<Guid>;