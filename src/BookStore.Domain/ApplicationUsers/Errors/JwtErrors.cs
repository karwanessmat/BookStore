using BookStore.SharedKernel.Abstractions.Errors;

namespace BookStore.Domain.ApplicationUsers.Errors;

public static class JwtErrors
{
    public static readonly Error NotFound =
        new(
            "Jwt.Found",
            "Token is empty.",
            ErrorType.NotFound);

    public static readonly Error Invalid =
        new(
            "Jwt.Invalid",
            "Invalid JWT algorithm.",
            ErrorType.Invalid);


    public static readonly Error Missing =
        new(
            "Jwt.Missing",
            "JWT missing sub or jti claim.",
            ErrorType.Problem);

    public static readonly Error Expired =
        new(
            "Jwt.Expired",
            "Expired.",
            ErrorType.Problem);

}