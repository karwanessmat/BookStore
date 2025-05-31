using System.Security.Claims;
using BookStore.Infrastructure.Abstractions.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookStore.Infrastructure.Abstractions.Authorization;
internal sealed class CustomClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not { IsAuthenticated: true } ||
            principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
        principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        using IServiceScope scope = serviceProvider.CreateScope();

        AuthorizationService authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

        string? userId = principal.GetUserId().ToString();

        UserRolesResponse userRoles = await authorizationService.GetRolesForUserAsync(userId);
        var principleValues = principal.Claims.Select(claim => claim.Value).ToList();

        var claimsIdentity = new ClaimsIdentity();


        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.UserId.ToString()));

        foreach (var role in userRoles.Roles.Where(role => !string.IsNullOrEmpty(role.Name) && !principleValues.Contains(role.Name)))
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}
