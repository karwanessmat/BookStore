using BookStore.Application.Abstractions.Interfaces.Authentication;
using BookStore.Domain.ApplicationUsers.Entities;
using BookStore.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Abstractions.Authentication;

internal sealed class EfTokenRevocationRepository(BookStoreAppContext db)
    : ITokenRevocationRepository
{
    public async Task<bool> ExistsAsync(string jti, CancellationToken ct = default)
    {
      var result = await db.RevokedTokens.AnyAsync(r => r.Jti == jti, ct);
      return result;
    }
       

    public async Task AddAsync(RevokedToken token, CancellationToken ct = default)
    {
        var revokedToken = RevokedToken.Create(token.Jti, token.ExpiryUtc);
        await db.RevokedTokens.AddAsync(revokedToken, ct);
        await db.SaveChangesAsync(ct);
    }


}