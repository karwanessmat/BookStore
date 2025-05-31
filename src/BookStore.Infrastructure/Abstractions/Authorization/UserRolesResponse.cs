using BookStore.Domain.ApplicationUsers.Entities;

namespace BookStore.Infrastructure.Abstractions.Authorization;
internal sealed class UserRolesResponse
{
    public Guid UserId { get; init; }

    public List<ApplicationRole> Roles { get; init; } = new();
}

