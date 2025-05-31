using FluentValidation;

namespace BookStore.Application.Users.User.Commands.Register;

internal sealed class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(c => c.Request.Email).NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Request.PhoneNumber).NotEmpty();
        RuleFor(c => c.Request.Password).NotEmpty();
        RuleFor(c => c.Request.ConfirmPassword).NotEmpty();
    }
}
