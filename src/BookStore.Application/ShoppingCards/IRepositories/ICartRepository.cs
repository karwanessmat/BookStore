using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Domain.ShoppingCards;
using BookStore.Domain.ShoppingCards.ValueObjects;

namespace BookStore.Application.ShoppingCards.IRepositories;

public interface ICartRepository : IRepository<Cart, CartId>
{
}