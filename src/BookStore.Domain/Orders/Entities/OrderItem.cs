using BookStore.Domain.BookAuthors;
using BookStore.Domain.Orders.ValueObjects.Ids;
using BookStore.Domain.ShoppingCards.Entities;
using BookStore.Domain.ShoppingCards.ValueObjects;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.Orders.Entities;
public sealed class OrderItem : Entity<OrderItemId>
{
    public Book Book { get; private set; }
    public string BookTitle { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal SubTotal => UnitPrice * Quantity;

    private OrderItem() { }

    private OrderItem(Book book, string title, decimal price, int qty, OrderItemId? id = null)
        : base(id ?? OrderItemId.CreateUnique())
    {
        Book = book;
        BookTitle = title;
        UnitPrice = price;
        Quantity = qty;
    }

    internal static OrderItem FromCartItem(CartItem c) =>
        new(c.Book, c.BookTitle, c.UnitPrice, c.Quantity);
}