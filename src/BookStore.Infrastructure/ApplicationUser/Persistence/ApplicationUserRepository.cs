using System.Security.Claims;
using BookStore.Application.Abstractions.Interfaces.Persistence.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserApp = BookStore.Domain.ApplicationUsers.ApplicationUser;

namespace BookStore.Infrastructure.ApplicationUser.Persistence;

public class ApplicationUserRepository(
    UserManager<UserApp> userManager, 
    SignInManager<UserApp> signInManager) : IUserRepository
{
    public async Task<UserApp?> GetUserByEmail(string email)
    {
        var result =await userManager.Users
            .Where(u=>u.Email!.Trim() == email.Trim())
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<UserApp?> GetUserById(string userId)
    {
        var result = await userManager.FindByIdAsync(userId);
        return result;
    }

    public async Task<(UserApp?, bool)> Register(UserApp applicationUser, string password)
    {
          
        var result = await  userManager.CreateAsync(applicationUser,password);
        return !result.Succeeded 
            ? (null,false) 
            : (user: applicationUser, true);
    }

    public async Task<bool> CheckPassword(UserApp appApplicationUser, string password)
    {
        return await userManager.CheckPasswordAsync(appApplicationUser, password);
    }

    public async Task<List<Claim>?> GetRolesAsync(UserApp applicationUser)
    {
       var roles = await userManager.GetRolesAsync(applicationUser);
       var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
       return roleClaims.ToList();
    }

    public async Task<List<Claim>?> GetClaimsAsync(UserApp applicationUser)
    {
        var claims = await userManager.GetClaimsAsync(applicationUser);
        return claims.ToList();
    }

    public async Task<bool> AddRoleAsync(UserApp createdNewUser, string role)
    {
       var result = await userManager.AddToRoleAsync(createdNewUser, role);
       return result.Succeeded;
    }
}