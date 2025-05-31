using BookStore.Application.Abstractions.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;

namespace BookStore.Infrastructure.Abstractions.Authentication;

internal sealed class TokenAccessor(IHttpContextAccessor ctxAcc) : ITokenAccessor
{
    public string? GetAccessToken()
    {
        // HttpContext could be null in background threads
        var ctx = ctxAcc.HttpContext;
        if (ctx is null) return null;

        var authHeader = ctx.Request.Headers.Authorization.ToString();       // faster than ["Authorization"]
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null;

        // return the part after "Bearer "
        return authHeader["Bearer ".Length..].Trim();
    }
}