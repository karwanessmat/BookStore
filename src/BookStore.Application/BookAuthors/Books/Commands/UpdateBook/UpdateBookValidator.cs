using FluentValidation;

namespace BookStore.Application.BookAuthors.Books.Commands.UpdateBook;

internal sealed class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookValidator()
    {
        // BookId must be a non-empty GUID
        RuleFor(x => x.Request.BookId)
            .NotEmpty().WithMessage("BookId is required.");

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

        // Description is optional, but if present, limit its length
        RuleFor(x => x.Request.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Request.Description))
            .WithMessage("Description cannot exceed 1000 characters.");

        // If PublishedDate is provided, it cannot lie in the future
        RuleFor(x => x.Request.PublishedDate)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow)
            .When(x => x.Request.PublishedDate.HasValue)
            .WithMessage("Published date cannot be in the future.");

        // ISBN is optional; if provided, check common ISBN-13 pattern
        RuleFor(x => x.Request.Isbn)
            .Matches(@"^(?:ISBN(?:-13)?:? )?(?=[0-9]{13}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)97[89][- ]?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9]$")
            .When(x => !string.IsNullOrWhiteSpace(x.Request.Isbn))
            .WithMessage("ISBN must be a valid ISBN-13 format.");

        // CoverImageUrl is optional; if provided, it should be a valid absolute URL
        RuleFor(x => x.Request.CoverImageUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .When(x => !string.IsNullOrWhiteSpace(x.Request.CoverImageUrl))
            .WithMessage("CoverImageUrl must be a valid absolute URL.");
    }
}