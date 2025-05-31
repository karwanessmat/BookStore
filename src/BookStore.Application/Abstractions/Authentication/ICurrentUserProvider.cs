using BookStore.Application.Abstractions.Models;

namespace BookStore.Application.Abstractions.Authentication;

public interface ICurrentUserProvider
{
    Guid GetUserId { get; }
    string UserName { get; }
    CurrentUser GetCurrentUser();
}
