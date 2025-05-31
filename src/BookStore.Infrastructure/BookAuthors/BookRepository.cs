using BookStore.Application.BookAuthors.IRepositories;
using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.Infrastructure.Shared.Persistence;
using BookStore.Infrastructure.Shared.Persistence.Repositories.Base;

namespace BookStore.Infrastructure.BookAuthors;

public class BookRepository(BookStoreAppContext logisticsDbContext)
    : Repository<Book, BookId>(logisticsDbContext), IBookRepository
{ }