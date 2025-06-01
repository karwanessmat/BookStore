using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.ShoppingCards.ValueObjects;

public sealed class CartItemId : ValueObject
{
    private CartItemId() { }
    public Guid Value { get; private set; }
    private CartItemId(Guid value)
    {
        Value = value;
    }

    public static CartItemId CreateUnique()
    {
        return new CartItemId(Guid.NewGuid());
    }

    public static CartItemId Create(Guid value)
    {
        return new CartItemId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}