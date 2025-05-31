using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.BookAuthors.ValueObjects;

public sealed class BookAuthorId : AggregateRootId<Guid>
{
    private BookAuthorId() { }

    public sealed override Guid Value { get; protected set; }

    private BookAuthorId(Guid value)
    {
        Value = value;
    }

    public static BookAuthorId CreateUnique()
    {
        return new BookAuthorId(Guid.NewGuid());
    }

    public static BookAuthorId Create(Guid value)
    {
        return new BookAuthorId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}