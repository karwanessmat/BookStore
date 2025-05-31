using System.Security.Claims;
using BookStore.Domain.ApplicationUsers;

namespace BookStore.Application.Abstractions.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user, IList<Claim>? roles);
}
