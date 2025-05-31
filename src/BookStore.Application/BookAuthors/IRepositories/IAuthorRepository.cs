using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.Entities;
using BookStore.Domain.BookAuthors.ValueObjects;

namespace BookStore.Application.BookAuthors.IRepositories;

public interface IAuthorRepository : IRepository<Author, AuthorId>
{
}