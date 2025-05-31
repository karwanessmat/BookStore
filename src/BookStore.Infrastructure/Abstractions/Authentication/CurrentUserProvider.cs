using System.Security.Claims;
using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Throw;

namespace BookStore.Infrastructure.Abstractions.Authentication;

internal sealed class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public Guid GetUserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ?? Guid.Empty;

    public string UserName =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserName() ?? "";


    public CurrentUser GetCurrentUser()
    {
  
        httpContextAccessor.HttpContext.ThrowIfNull();

        var id = GetUserId;

        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(Id: id, Roles: roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .Distinct()
            .ToList();
    }
}
