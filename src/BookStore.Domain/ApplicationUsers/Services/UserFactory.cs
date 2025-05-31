using Microsoft.AspNetCore.Identity;

namespace BookStore.Domain.ApplicationUsers.Services;

public static class PasswordFactory
{
    private static readonly PasswordHasher<ApplicationUser> PasswordHasher = new();

    public static void Create(ApplicationUser applicationUser,string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            password = GenerateRandomPassword(6);
        }

        // Hash the password
        string? hashedPassword = PasswordHasher.HashPassword(applicationUser, password);
        applicationUser.PasswordHash = hashedPassword; // Set the hashed password

    }

    private static string GenerateRandomPassword(int length)
    {
        // You can include more/less characters here as needed
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        // Create a random string of the specified length
        return new string(
            Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray()
        );
    }
}
