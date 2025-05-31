using BookStore.Application.Abstractions.Authentication;
using BookStore.Domain.ApplicationUsers;
using BookStore.SharedKernel.Abstractions.IServices;
using BookStore.SharedKernel.User;

namespace BookStore.Application.Abstractions.Extensions;
public static class IsAuthorizedExtensions
{
    public static bool IsAuthorized(this IUser userService, ICurrentUserProvider currentUser)
    {

        var user = currentUser.GetCurrentUser();
        var userInAdminRole = user.Roles.Any(x => x == RoleName.Admin);

        if (userInAdminRole)
        {
            return true;
        }



        return userService switch
        {
            ApplicationUser applicationUser => applicationUser.Id == user.Id,
            _ => false
        };


        //return userService.Office.Id == user.Id;
    }
}
