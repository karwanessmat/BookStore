using FluentValidation;

namespace BookStore.Application.BookAuthors.Authors.Commands.UpdateAuthor;

internal sealed class UpdateAuthorValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorValidator()
    {
        RuleFor(x => x.Request.AuthorId)
            .NotEmpty().WithMessage("AuthorId is required.");

        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage("Author name is required.")
            .MaximumLength(100).WithMessage("Author name must be at most 100 characters.");
    }
}