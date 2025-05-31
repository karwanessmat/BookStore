using System.Security.Claims;
using BookStore.Domain.ApplicationUsers;

namespace BookStore.Application.Abstractions.Interfaces.Persistence.User;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByEmail(string email);
    Task<ApplicationUser?> GetUserById(string userId);
    Task<(ApplicationUser? user, bool)> Register(ApplicationUser applicationUser, string password);
    Task<bool> CheckPassword(ApplicationUser appApplicationUser, string password);
    Task<List<Claim>?> GetRolesAsync(ApplicationUser applicationUser);
    Task<List<Claim>?> GetClaimsAsync(ApplicationUser applicationUser);
    Task<bool> AddRoleAsync(ApplicationUser? createdNewUser, string role);
}
