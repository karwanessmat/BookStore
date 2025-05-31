using BookStore.Contracts.Users;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Application.Abstractions.Interfaces.Authentication;

public interface IJwtTokenReader
{
    Result<JwtPayload> TryRead(string jwt, bool ignoreExpiry = false);
}