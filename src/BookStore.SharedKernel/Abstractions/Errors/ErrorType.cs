namespace BookStore.SharedKernel.Abstractions.Errors;

public enum ErrorType
{
    Failure = 1,
    Validation = 2,
    Problem = 3,
    NotFound = 4,
    Conflict = 5,
    Forbidden =6,
    Invalid = 7,
    Unexpected =8,
    Unauthorized = 9
}
