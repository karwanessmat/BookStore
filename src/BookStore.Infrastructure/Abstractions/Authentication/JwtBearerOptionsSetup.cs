using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Application.Abstractions.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Infrastructure.Abstractions.Authentication;


internal sealed class JwtBearerOptionsSetup(IOptions<JwtSettings> jwtSettings)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
            {
                DateTime currentUtcTime = DateTime.UtcNow;
                bool result = currentUtcTime < expires;

                // Additional custom validation logic
                return result;
            }
        };


        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async ctx =>
            {
                var jti = ctx.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
                if (jti is null)
                {
                    ctx.Fail("Missing jti");
                    return;
                }

                var repo = ctx.HttpContext.RequestServices.GetRequiredService<ITokenRevocationRepository>();
                if (await repo.ExistsAsync(jti, ctx.HttpContext.RequestAborted))
                    ctx.Fail("Token revoked");
            }
        };

        //These properties are particularly important when using protocols such as OpenID Connect or when integrating with identity providers that publish their configurations. 
        options.MetadataAddress = _jwtSettings.MetadataUrl;
        options.RequireHttpsMetadata = _jwtSettings.RequireHttpsMetadata;


    }


    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
