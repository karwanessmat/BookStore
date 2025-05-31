using BookStore.SharedKernel.Books;

namespace BookStore.Contracts.Books;

public record BookResponse(
    Guid BookId,
    string Title,
    decimal Price,
    bool IsAvailable,
    int StockQuantity,
    string? Description,
    DateTimeOffset? PublishedDate,
    string? Isbn,
    string? CoverImageUrl,
    DateTimeOffset CreatedDate,
    DateTimeOffset LastModifiedDate,
    List<BookAuthorsResponse> AuthorsResponses);


public record BookAuthorsResponse(string Author, Gender Genre);