using BookStore.Application.Abstractions.Helpers;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Books;

namespace BookStore.Application.BookAuthors.Books.Queries.GetBookAuthors;

public sealed record GetBookAuthorsQuery(BookQueryParameters Parameters) : IQuery<PagedList<BookResponse>>;