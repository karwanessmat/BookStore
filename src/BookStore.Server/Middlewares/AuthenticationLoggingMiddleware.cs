using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Infrastructure.Abstractions.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Server.Middlewares;

public class JwtDiagnosticMiddleware(
    RequestDelegate next,
    ILogger<JwtDiagnosticMiddleware> logger,
    IOptions<JwtSettings> jwtSettings)
{
    // The same key used for signing tokens

    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        string? token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
        logger.LogInformation("Received JWT Token: {Token}", token);

        if (string.IsNullOrEmpty(token))
        {
            logger.LogWarning("JWT Token is missing from the request.");
        }
        else
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                if (!tokenHandler.CanReadToken(token))
                {
                    logger.LogError("JWT Token cannot be read and may be malformed.");
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Invalid JWT Token");
                    return;
                }

                var validationParameters = new TokenValidationParameters
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

                ClaimsPrincipal? principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken? validatedToken);
                logger.LogInformation("JWT Token is valid. Principal Name: {Name}", principal.Identity.Name);
            }
            catch (SecurityTokenExpiredException)
            {
                logger.LogError("JWT Token has expired.");
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Expired JWT Token");
            }
            catch (SecurityTokenException stEx)
            {
                logger.LogError("JWT Token validation failed: {Message}", stEx.Message);
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid JWT Token");
            }
            catch (Exception ex)
            {
                logger.LogError("Error processing JWT Token: {Message}", ex.Message);
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync("Error processing JWT Token");
            }
        }

        await next(context);
    }

}