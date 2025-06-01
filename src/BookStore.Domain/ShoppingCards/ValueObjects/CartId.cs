using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.ShoppingCards.ValueObjects;

public sealed class CartId : AggregateRootId<Guid>
{
    private CartId() { }

    public override Guid Value { get; protected set; }

    private CartId(Guid value) => Value = value;

    public static CartId CreateUnique() => new(Guid.NewGuid());
    public static CartId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}