using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.Orders.ValueObjects.Ids;

public sealed class OrderId : AggregateRootId<Guid>
{
    private OrderId() { }

    public override Guid Value { get; protected set; }

    private OrderId(Guid value) => Value = value;

    public static OrderId CreateUnique() => new(Guid.NewGuid());
    public static OrderId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}