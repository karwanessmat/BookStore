﻿using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.BookAuthors.IRepositories;
using BookStore.Application.Orders.IRepositories;
using BookStore.Application.ShoppingCards.IRepositories;
using BookStore.Infrastructure.BookAuthors;
using BookStore.Infrastructure.Orders;
using BookStore.Infrastructure.ShoppingCards;

namespace BookStore.Infrastructure.Shared.Persistence.Repositories.Base;

public class RepositoryManager(BookStoreAppContext spardaDbContext) : IRepositoryManager
{
    // Lazy initialization for repositories
    private readonly Lazy<IBookRepository> _bookRepository = new(() => new BookRepository(spardaDbContext));
    private readonly Lazy<IAuthorRepository> _authorsRepository = new(() => new AuthorRepository(spardaDbContext));
    private readonly Lazy<ICartRepository> _cartRepository= new(() => new CartRepository(spardaDbContext));
    private readonly Lazy<IOrderRepository> _orderRepository= new(() => new OrderRepository(spardaDbContext));

     public IBookRepository Books => _bookRepository.Value;
    public IAuthorRepository Authors => _authorsRepository.Value;
    public ICartRepository Carts => _cartRepository.Value;
    public IOrderRepository Orders => _orderRepository.Value;


    public async ValueTask DisposeAsync()
    {
        await spardaDbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}