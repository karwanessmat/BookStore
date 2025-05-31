// JwtTokenGenerator.cs

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Application.Abstractions.Interfaces.Authentication;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Infrastructure.Abstractions.Authentication;

public sealed class JwtTokenGenerator(
    IDateTimeProvider clock,
    IOptions<JwtSettings> options,
    ILogger<JwtTokenGenerator> log)
    : IJwtTokenGenerator
{
    private readonly JwtSettings _jwt = options.Value;
    private readonly ILogger _log = log;

    public string GenerateToken(Domain.ApplicationUsers.ApplicationUser user, IList<Claim>? roleClaims)
    {
        var jti = Guid.NewGuid().ToString();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,       user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email,     user.Email),
            new(JwtRegisteredClaimNames.GivenName, user.FullName),
            new(JwtRegisteredClaimNames.Jti,       jti)
        }.Union(roleClaims ?? Enumerable.Empty<Claim>());


        // Assume Erbil = Asia/Baghdad (+03:00)

        // ❶ Get “Erbil now” *with the offset attached*
        DateTimeOffset localNow =clock.GetCurrentDateTimeOffsetByTimeZone().GetAwaiter().GetResult();  // e.g. 18:39:43 +03:00

        // ❷ Add your lifetime on that same offset
        DateTimeOffset localExpiry = localNow.AddMinutes(_jwt.ExpiryMinutes);  // 19:39:43 +03:00

        //var token = new JwtSecurityToken(
        //    issuer: _jwt.Issuer,
        //    audience: _jwt.Audience,
        //    claims: claims,
        //    expires: expire,
        //    signingCredentials: creds);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: localNow.UtcDateTime,   // 15:39:43 Z
            expires: localExpiry.UtcDateTime,  // 16:39:43 Z
            signingCredentials: creds);


        _log.LogDebug("Issued JWT for {UserId} expiring at {Utc}", user.Id, token.ValidTo);
        var result = new JwtSecurityTokenHandler().WriteToken(token);
        return result;
    }
}