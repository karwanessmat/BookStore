using System.Security.Claims;

namespace BookStore.Contracts.Users;

public sealed record JwtPayload(
    string UserId,        // sub
    string Jti,           // unique token id
    DateTime ExpiryUtc,
    IReadOnlyCollection<Claim> Claims);