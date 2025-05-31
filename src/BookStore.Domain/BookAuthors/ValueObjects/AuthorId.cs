using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.BookAuthors.ValueObjects;

public class AuthorId : AggregateRootId<Guid>
{
    private AuthorId() { }

    public sealed override Guid Value { get; protected set; }

    private AuthorId(Guid value)
    {
        Value = value;
    }

    public static AuthorId CreateUnique()
    {
        return new AuthorId(Guid.NewGuid());
    }

    public static AuthorId Create(Guid value)
    {
        return new AuthorId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}