using BookStore.Application.BookAuthors.Books.Commands.CreateBook;
using FluentValidation;

namespace BookStore.Application.BookAuthors.Authors.Commands.CreateAuthor;
internal sealed class CreateAuthorValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage("Author name is required.")
            .MaximumLength(100).WithMessage("Author name must be at most 100 characters.");


    }
}