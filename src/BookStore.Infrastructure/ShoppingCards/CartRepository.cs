using BookStore.Application.ShoppingCards.IRepositories;
using BookStore.Domain.ShoppingCards;
using BookStore.Domain.ShoppingCards.ValueObjects;
using BookStore.Infrastructure.Shared.Persistence;
using BookStore.Infrastructure.Shared.Persistence.Repositories.Base;

namespace BookStore.Infrastructure.ShoppingCards;

public class CartRepository(BookStoreAppContext logisticsDbContext)
    : Repository<Cart, CartId>(logisticsDbContext), ICartRepository
{ }