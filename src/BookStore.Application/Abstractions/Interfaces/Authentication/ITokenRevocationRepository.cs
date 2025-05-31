using BookStore.Domain.ApplicationUsers.Entities;

namespace BookStore.Application.Abstractions.Interfaces.Authentication;

public interface ITokenRevocationRepository
{
    Task<bool> ExistsAsync(string jti, CancellationToken ct = default);
    Task AddAsync(RevokedToken token, CancellationToken ct = default);
}