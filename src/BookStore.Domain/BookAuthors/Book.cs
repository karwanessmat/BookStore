using BookStore.Domain.BookAuthors.Entities;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.BookAuthors;


public sealed class Book : AggregateRoot<BookId, Guid>
{
    public DateTimeOffset CreatedDate { get; }
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset? PublishedDate { get; private set; }
    public string? Isbn { get; private set; }
    public string? CoverImageUrl { get; private set; }

    public bool IsAvailable { get; private set; }

    private readonly List<Author> _authors = new();
    public IReadOnlyCollection<Author> Authors => _authors.AsReadOnly();

    private Book() { }

    private Book(DateTimeOffset created, string title, decimal price, int stock,
                 IEnumerable<Author> authors,
                 string? description, DateTimeOffset? published,
                 string? isbn, string? cover, BookId? id = null)
        : base(id ?? BookId.CreateUnique())
    {
        if (price < 0) throw new ArgumentException("Price must be ≥ 0.", nameof(price));
        if (stock < 0) throw new ArgumentException("Stock must be ≥ 0.", nameof(stock));
        if (!authors?.Any() ?? true)
            throw new ArgumentException("At least one authorId is required.", nameof(authors));

        CreatedDate = created;
        Title = title.Trim();
        Price = price;
        StockQuantity = stock;
        Description = description?.Trim();
        PublishedDate = published;
        Isbn = isbn?.Trim();
        CoverImageUrl = cover?.Trim();
        _authors = authors.ToList();
        IsAvailable = stock > 0;
    }

    public static Book Create(
        DateTimeOffset created,
        string title,
        decimal price,
        int stockQuantity,
        IEnumerable<Author> authors,
        string? description = null,
        DateTimeOffset? publishedDate = null,
        string? isbn = null,
        string? coverImageUrl = null,
        BookId? id = null)
    {
        var result = new Book(created, title, price, stockQuantity, authors,
            description, publishedDate, isbn, coverImageUrl, id);
        return result;
    }


    public void AddAuthor(Author id) => _authors.Add(id);
    public void RemoveAuthor(Author id) => _authors.Remove(id);

    public void Update(
        string newTitle,
        decimal newPrice,
        int newStockQuantity,
        string? newDescription,
        DateTimeOffset? newPublishedDate,
        string? newIsbn,
        string? newCoverImageUrl,
        IEnumerable<Author>? newAuthors = null)      
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty.", nameof(newTitle));
        if (newPrice < 0) throw new ArgumentException("Price must be ≥ 0.", nameof(newPrice));
        if (newStockQuantity < 0) throw new ArgumentException("Stock must be ≥ 0.", nameof(newStockQuantity));

        Title = newTitle.Trim();
        Price = newPrice;
        StockQuantity = newStockQuantity;
        Description = newDescription?.Trim();
        PublishedDate = newPublishedDate;
        Isbn = newIsbn?.Trim();
        CoverImageUrl = newCoverImageUrl?.Trim();
        IsAvailable = newStockQuantity > 0;

        if (newAuthors is not null)
        {
            if (!newAuthors.Any())
                throw new ArgumentException("Book must have at least one author.", nameof(newAuthors));

            _authors.Clear();
            _authors.AddRange(newAuthors.Distinct());
        }
    }

    public void DecreaseStock(int by)
    {
        if (by <= 0)
            throw new ArgumentOutOfRangeException(nameof(by));

        StockQuantity = Math.Max(0, StockQuantity - by);
        IsAvailable = StockQuantity > 0;
    }
}
