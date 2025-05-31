using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Books;
using BookStore.Domain.BookAuthors.ValueObjects;

namespace BookStore.Application.BookAuthors.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(BookId BookId) : IQuery<BookResponse>;