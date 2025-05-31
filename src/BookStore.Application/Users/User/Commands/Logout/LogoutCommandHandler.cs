

using BookStore.Application.Abstractions.Interfaces.Authentication;
using BookStore.Domain.ApplicationUsers.Entities;
using BookStore.SharedKernel.Abstractions;
using MediatR;

namespace BookStore.Application.Users.User.Commands.Logout;

public sealed class LogoutCommandHandler(
    IJwtTokenReader tokenReader,            
    ITokenRevocationRepository revokedRepo) : IRequestHandler<LogoutCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken ct)
    {
        var parseResult = tokenReader.TryRead(request.AccessToken, ignoreExpiry: true);
        if (parseResult.IsFailure)
            return Result.Failure<bool>(parseResult.Error);

        var jti = parseResult.Value.Jti;
        var exp = parseResult.Value.ExpiryUtc;

        if (await revokedRepo.ExistsAsync(jti, ct))
            return Result.Success(true);             

        var revoked = RevokedToken.Create(jti, exp);
        await revokedRepo.AddAsync(revoked, ct);

        return Result.Success(true);
    }
}
