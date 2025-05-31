using FluentValidation;

namespace BookStore.Application.BookAuthors.Books.Commands.DeleteBook;

internal sealed class DeleteBookValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookValidator()
    {
        // Ensure BookID is not an empty GUID
        RuleFor(x => x.BookId)
            .NotEmpty()
            .WithMessage("BookId is required for deletion.");
    }
}