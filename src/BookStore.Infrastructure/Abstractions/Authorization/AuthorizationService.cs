using BookStore.Domain.ApplicationUsers.Entities;
using BookStore.Infrastructure.Shared.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Infrastructure.Abstractions.Authorization;


internal sealed class AuthorizationService(
    BookStoreAppContext dbContext,
    UserManager<Domain.ApplicationUsers.ApplicationUser> userManager)
{
    public async Task<UserRolesResponse> GetRolesForUserAsync(string userId)
    {


        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException($"No user found with ID {userId}.");
        }


        IList<string>? userRoles = await userManager.GetRolesAsync(user);
        var rolesList = userRoles.Select(roleName => new ApplicationRole(roleName)).ToList();

        var rolesResponse = new UserRolesResponse
        {
            UserId = user.Id,
            Roles = rolesList
        };


        return rolesResponse;
    }


}
