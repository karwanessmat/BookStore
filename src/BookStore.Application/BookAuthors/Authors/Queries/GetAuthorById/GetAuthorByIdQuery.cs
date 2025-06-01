using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Authors;

namespace BookStore.Application.BookAuthors.Authors.Queries.GetAuthorById;

public sealed record GetAuthorByIdQuery(Guid AuthorId) : IQuery<AuthorResponse>;