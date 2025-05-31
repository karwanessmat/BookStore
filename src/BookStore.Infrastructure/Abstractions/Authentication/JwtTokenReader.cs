// ──────────────────────────────────────────────────────────────
//  JwtTokenReader.cs        (Infrastructure.Abstractions)
// ──────────────────────────────────────────────────────────────

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Application.Abstractions.Interfaces.Authentication;
using BookStore.Domain.ApplicationUsers.Errors;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtPayload = BookStore.Contracts.Users.JwtPayload;

namespace BookStore.Infrastructure.Abstractions.Authentication;

internal sealed class JwtTokenReader(IOptions<JwtSettings> jwtOptions,IDateTimeProvider clock) : IJwtTokenReader
{
    private readonly JwtSettings _jwt = jwtOptions.Value;
    private readonly JwtSecurityTokenHandler _handler = new()
    {
        // Keep original JWT claim names (sub, jti, role, …)
        MapInboundClaims = false
    };

    public Result<JwtPayload> TryRead(string jwt, bool ignoreExpiry = false)
    {
        if (string.IsNullOrWhiteSpace(jwt))
            return Result.Failure<JwtPayload>(JwtErrors.NotFound);

        try
        {
            var principal = _handler.ValidateToken(jwt, BuildParameters(ignoreExpiry), out var validated);

            // Accept only HS256 tokens issued by us
            if (validated is not JwtSecurityToken jwtToken ||
                jwtToken.Header.Alg is not SecurityAlgorithms.HmacSha256)
                return Result.Failure<JwtPayload>(JwtErrors.Invalid);

            var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var expUtc = jwtToken.ValidTo;              // already UTC

            if (userId is null || jti is null)
                return Result.Failure<JwtPayload>(JwtErrors.Missing);

            return Result.Success(new JwtPayload(
                UserId: userId,
                Jti: jti,
                ExpiryUtc: expUtc,
                Claims: principal.Claims.ToList()));
        }
        catch (SecurityTokenExpiredException)
        {
            return Result.Failure<JwtPayload>(JwtErrors.Expired);
        }
        catch (SecurityTokenException)
        {
            return Result.Failure<JwtPayload>(JwtErrors.Invalid);
        }
        catch (Exception)
        {
            return Result.Failure<JwtPayload>(JwtErrors.Invalid);
        }
    }

    // --------------------------------------------------------------------
    // helpers
    // --------------------------------------------------------------------
    private TokenValidationParameters BuildParameters(bool ignoreExpiry) => new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = _jwt.Issuer,
        ValidAudience = _jwt.Audience,

        ValidateLifetime = !ignoreExpiry,
        ClockSkew = TimeSpan.Zero,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(_jwt.SecretKey)),

        // Tell ASP.NET Core which claim represents Name and Role
        NameClaimType = JwtRegisteredClaimNames.Sub,
        RoleClaimType = "role"
    };
}
