
using BookStore.Application.BookAuthors.IRepositories;

namespace BookStore.Application.Abstractions.Interfaces.Persistence.Base;

public interface IRepositoryManager : IAsyncDisposable
{
    public IBookRepository Books { get; }
    public IAuthorRepository Authors { get; }

}