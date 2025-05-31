using BookStore.Application.BookAuthors.IRepositories;
using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.Entities;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.Infrastructure.Shared.Persistence;
using BookStore.Infrastructure.Shared.Persistence.Repositories.Base;

namespace BookStore.Infrastructure.BookAuthors;

public class AuthorRepository(BookStoreAppContext logisticsDbContext)
    : Repository<Author, AuthorId>(logisticsDbContext), IAuthorRepository
{ }