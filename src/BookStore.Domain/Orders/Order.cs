using BookStore.Domain.Orders.Entities;
using BookStore.Domain.Orders.ValueObjects;
using BookStore.Domain.Orders.ValueObjects.Ids;
using BookStore.Domain.ShoppingCards;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Orders;

namespace BookStore.Domain.Orders;

public sealed class Order : AggregateRoot<OrderId, Guid>
{
    public Guid UserId { get; private set; }
    public DateTimeOffset OrderedDate { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public ShippingAddress ShippingAddress { get; private set; } 
    public decimal ShippingCost { get; private set; }

    public decimal Total => _items.Sum(i => i.SubTotal);

    // EF Core ctor
    private Order() { }

    private Order(
        Guid userId, 
        DateTimeOffset orderedDate, 
        OrderStatus status,
        ShippingAddress shippingAddress,
        decimal shippingCost,
        OrderId? id = null)
        : base(id ?? OrderId.CreateUnique())
    {
        UserId = userId;
        OrderedDate = orderedDate;
        Status = status;
        ShippingAddress = shippingAddress;
        ShippingCost = shippingCost;
    }

    public static Order CreateFromCart(
        Cart cart, 
        DateTimeOffset nowUtc,
        ShippingAddress shippingAddress,
        decimal shippingCost)
    {
        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty – cannot create order.");

        var order = new Order(
            cart.UserId,
            nowUtc,
            OrderStatus.Pending,
            shippingAddress,
            shippingCost);
        foreach (var ci in cart.Items)
            order._items.Add(OrderItem.FromCartItem(ci));
     
        return order;
    }

    // state-change helpers
    public void MarkPaid() => Status = OrderStatus.Paid;
    public void MarkShipped() => Status = OrderStatus.Shipped;
    public void MarkCompleted() => Status = OrderStatus.Completed;
    public void Cancel() => Status = OrderStatus.Cancelled;
}