using System.Security.Claims;

namespace BookStore.Infrastructure.Abstractions.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out Guid parsedUserId)
                        ? parsedUserId
                        :Guid.Empty;
    }




    public static string GetUserName(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.GivenName) ?? throw new ApplicationException("User Name is unavailable");
    }
}
