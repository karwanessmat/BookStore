using BookStore.Domain.ApplicationUsers;

namespace BookStore.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(
        ApplicationUser applicationUser,
        string password,
        CancellationToken cancellationToken = default);
}
