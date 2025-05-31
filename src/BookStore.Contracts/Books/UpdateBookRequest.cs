namespace BookStore.Contracts.Books;

public record UpdateBookRequest(
    Guid BookId,
    string Title,
    decimal Price,
    int StockQuantity,
    string? Description,
    DateTimeOffset? PublishedDate,
    string? Isbn,
    string? CoverImageUrl);