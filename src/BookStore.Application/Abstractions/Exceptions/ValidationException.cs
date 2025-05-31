namespace BookStore.Application.Abstractions.Exceptions;

public sealed class ValidationException(IEnumerable<ValidationError> errors) : Exception
{
    public IEnumerable<ValidationError> Errors { get; set; } = errors;
}
