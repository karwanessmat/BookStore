namespace BookStore.Contracts.Users;

public record RegisterRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password, 
    string ConfirmPassword);