using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.Domain.ShoppingCards.Entities;
using BookStore.Domain.ShoppingCards.ValueObjects;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.ShoppingCards;

public sealed class Cart : AggregateRoot<CartId, Guid>
{
    public Guid UserId { get; private set; }
    public DateTimeOffset CreatedDate { get; }

    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public decimal Total => _items.Sum(i => i.SubTotal);

    private Cart() { }

    private Cart(Guid userId, CartId? id = null)
        : base(id ?? CartId.CreateUnique())
    {
        UserId = userId;
        CreatedDate = DateTimeOffset.UtcNow;
    }

    public static Cart Create(Guid userId, CartId? id = null) => new(userId, id);

    public void AddItem(Book book, string title, decimal price, int qty = 1)
    {
        var existing = _items.FirstOrDefault(i => i.Book.Id == book.Id);
        if (existing is not null)
        {
            existing.Increase(qty);
            return;
        }
        _items.Add(CartItem.Create(book, title, price, qty));
    }

    public void UpdateQuantity(CartItemId itemId, int qty)
    {
        var item = _items.Single(i => i.Id == itemId);
        item.Set(qty);
    }

    public void RemoveItem(CartItemId itemId)
    {
        var item = _items.Single(i => i.Id == itemId);
        _items.Remove(item);    
    }

    public void Clear() => _items.Clear();
}