namespace BookStore.Application.Abstractions.Exceptions;

public sealed class ConcurrencyException(string message, Exception innerException)
    : Exception(message, innerException);
