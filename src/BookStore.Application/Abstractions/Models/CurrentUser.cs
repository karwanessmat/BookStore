namespace BookStore.Application.Abstractions.Models;

public record CurrentUser(
    Guid Id,
    IReadOnlyList<string> Roles);
