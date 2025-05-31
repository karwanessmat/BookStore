using BookStore.Application.Abstractions.Interfaces.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.User;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.ApplicationUsers.Errors;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Application.Users.User.Commands.Login;

internal sealed class LogInUserCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator, 
    IUserRepository userRepository) : ICommandHandler<LogInUserCommand, string>
{
    public async Task<Result<string>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var (email, password) = request;

        var appUser = await userRepository.GetUserByEmail(email);


        if (appUser is null)
        {
            return Result.Failure<string>(UserErrors.NotFound);
        }

        var checkingPassword = await userRepository.CheckPassword(appUser, password);
        if (!checkingPassword)
        {
            return Result.Failure<string>(UserErrors.InvalidCredentials);
        }

        if (appUser is { IsActive: false })
        {
            return Result.Failure<string>(UserErrors.InactiveUser);
        }

        var roles = await userRepository.GetRolesAsync(appUser);

        var token = jwtTokenGenerator.GenerateToken(appUser, roles);

        return token;
    }
}
