using FluentValidation;

namespace BookStore.Application.BookAuthors.Authors.Commands.DeleteAuthor;

internal sealed class DeleteAuthorValidator : AbstractValidator<DeleteAuthorCommand>
{
    public DeleteAuthorValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("AuthorId is required for deletion.");
    }
}