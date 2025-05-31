using BookStore.Domain.ApplicationUsers.ValueObjects;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.ApplicationUsers.Entities;

/// <summary>
/// A record of an access-token that was explicitly invalidated before expiring.
/// </summary>
public sealed class RevokedToken : Entity<RevokedTokenId>       // Entity<TId> in your base library
{
    private RevokedToken() { }                       // for EF Core

    private RevokedToken(
        string jti, 
        DateTimeOffset expiryUtc,
        RevokedTokenId? id = null)
        : base(id ?? RevokedTokenId.CreateUnique())
    {
        Jti = jti;
        ExpiryUtc = expiryUtc;
    }

    public string Jti { get; private set; }
    public DateTimeOffset ExpiryUtc { get; private set; }

    public static RevokedToken Create(string jti, DateTimeOffset expiryUtc, RevokedTokenId? id = null)
    {
        return new RevokedToken(jti, expiryUtc, id);
    }
        
}