using BookStore.Application.Abstractions.Helpers;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Authors;

namespace BookStore.Application.BookAuthors.Authors.Queries.GetAuthors;

public sealed record GetAuthorsQuery(AuthorQueryParameters Parameters) : IQuery<PagedList<AuthorResponse>>;