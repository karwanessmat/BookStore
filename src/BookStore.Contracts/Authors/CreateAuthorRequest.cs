using BookStore.SharedKernel.Books;

namespace BookStore.Contracts.Authors;

public record CreateAuthorRequest(
    string Name,
    Gender Gender,
    string? Bio);