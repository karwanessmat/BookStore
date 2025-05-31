using FluentValidation;

namespace BookStore.Application.BookAuthors.Books.Commands.CreateBook
{
    internal sealed class CreateBookValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookValidator()
        {
            // Title must be provided and reasonably sized
            RuleFor(x => x.Request.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");


            // Price must be non-negative
            RuleFor(x => x.Request.Price)
                .GreaterThanOrEqualTo(0m).WithMessage("Price must be at least 0.");

            // StockQuantity must be zero or positive
            RuleFor(x => x.Request.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be at least 0.");

        }
    }
}
