

using BookStore.SharedKernel.Abstractions.Errors;

namespace BookStore.Domain.BookAuthors.Errors
{
    public static class BookErrors
    {
        public static readonly Error NotFound =
            new(
                "Book.NotFound",
                "The book with the specified identifier was not found",
                ErrorType.NotFound);

        public static readonly Error DuplicateIsbn =
            new(
                "Book.DuplicateIsbn",
                "A book with the specified ISBN already exists",
                ErrorType.Conflict);

        public static readonly Error InvalidPrice =
            new(
                "Book.InvalidPrice",
                "The provided book price is invalid",
                ErrorType.Validation);

        public static readonly Error InvalidStockQuantity =
            new(
                "Book.InvalidStockQuantity",
                "The provided stock quantity is invalid",
                ErrorType.Validation);

        public static readonly Error UnauthorizedAccess =
            new(
                "Book.UnauthorizedAccess",
                "You are not authorized to modify this book",
                ErrorType.Unauthorized);

        public static readonly Error DeletionFailed =
            new(
                "Book.DeletionFailed",
                "The book could not be deleted",
                ErrorType.Conflict);
    }
}