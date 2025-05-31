namespace BookStore.Contracts.Books;

public record CreateBookRequest(
    string Title,
    decimal Price,
    int StockQuantity,
    IEnumerable<Guid> AuthorIds,
    string? Description,
    DateTimeOffset? PublishedDate,
    string? Isbn,
    string? CoverImageUrl);