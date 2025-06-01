
using BookStore.Application.BookAuthors.IRepositories;
using BookStore.Application.ShoppingCards.IRepositories;

namespace BookStore.Application.Abstractions.Interfaces.Persistence.Base;

public interface IRepositoryManager : IAsyncDisposable
{
    public IBookRepository Books { get; }
    public IAuthorRepository Authors { get; }
    public ICartRepository Carts { get;  }
}