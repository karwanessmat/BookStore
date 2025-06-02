using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.Orders.ValueObjects.Ids;

public sealed class OrderItemId : ValueObject
{
    private OrderItemId() { }
    public Guid Value { get; private set; }
    private OrderItemId(Guid value)     
    {
        Value = value;
    }

    public static OrderItemId CreateUnique()
    {
        return new OrderItemId(Guid.NewGuid());
    }

    public static OrderItemId Create(Guid value)
    {
        return new OrderItemId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}