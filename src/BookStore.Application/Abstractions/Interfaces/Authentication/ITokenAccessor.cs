namespace BookStore.Application.Abstractions.Interfaces.Authentication;

public interface ITokenAccessor
{
    /// <summary>
    /// Reads the Authorization header from the current request and returns the naked JWT
    /// </summary>
    string? GetAccessToken();
}