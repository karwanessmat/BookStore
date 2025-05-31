using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;


namespace BookStore.Domain.BookAuthors.Entities;

// BookAuthor join entity
public sealed class BookAuthor
{
    public AggregateRootId<Guid> BookId { get; set; }
    public AggregateRootId<Guid> AuthorId { get; set; }
}
