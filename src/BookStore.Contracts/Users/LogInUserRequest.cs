namespace BookStore.Contracts.Users;

public sealed record LogInUserRequest(string Email, string Password);