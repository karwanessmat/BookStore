using BookStore.SharedKernel.Books;

namespace BookStore.Contracts.Authors;

public record UpdateAuthorRequest(
    Guid AuthorId,
    string Name,
    Gender Gender,
    string? Bio);