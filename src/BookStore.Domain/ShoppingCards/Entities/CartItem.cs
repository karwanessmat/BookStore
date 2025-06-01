using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.Domain.ShoppingCards.ValueObjects;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.ShoppingCards.Entities;

public sealed class CartItem : Entity<CartItemId>
{
    public string BookTitle { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal SubTotal => UnitPrice * Quantity;
    public Book Book { get; private set; }

    private CartItem() { }

    private CartItem(
        Book book,
        string bookTitle,
        decimal unitPrice,
        int quantity,
    CartItemId? id = null)
        : base(id ?? CartItemId.CreateUnique())
    {
        Book= book;
        BookTitle = bookTitle;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public static CartItem Create(Book book, string title, decimal price, int qty,CartItemId? id=null)
        => new(book, title, price, qty,null);

    internal void Increase(int by) => Quantity += by;
    internal void Set(int qty) => Quantity = qty;
}