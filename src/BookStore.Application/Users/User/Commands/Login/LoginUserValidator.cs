using FluentValidation;

namespace BookStore.Application.Users.User.Commands.Login;

internal sealed class LoginUserValidator: AbstractValidator<LogInUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty();

        RuleFor(c => c.Password).NotEmpty();
        RuleFor(c => c.Password)
            .MinimumLength(6);
    }
}
