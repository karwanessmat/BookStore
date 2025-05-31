using BookStore.SharedKernel.Abstractions.Errors;

namespace BookStore.Domain.BookAuthors.Errors;

public static class AuthorErrors
{
    public static readonly Error NotFound =
        new(
            "Author.NotFound",
            "The author with the specified identifier was not found",
            ErrorType.NotFound);


    public static readonly Error AuthorIdRequired =
        new(
            "Author.AuthorIdRequired",
            "At least one authorId must be supplied.",
            ErrorType.Validation);

    public static readonly Error DuplicateName =
        new(
            "Author.DuplicateName",
            "An author with the specified name already exists",
            ErrorType.Conflict);

    public static readonly Error InvalidName =
        new(
            "Author.InvalidName",
            "The provided author name is invalid",
            ErrorType.Validation);

    public static readonly Error InvalidGender =
        new(
            "Author.InvalidGender",
            "The provided author gender is invalid",
            ErrorType.Validation);

    public static readonly Error UnauthorizedAccess =
        new(
            "Author.UnauthorizedAccess",
            "You are not authorized to modify this author",
            ErrorType.Unauthorized);

    public static readonly Error DeletionFailed =
        new(
            "Author.DeletionFailed",
            "The author could not be deleted",
            ErrorType.Conflict);
}