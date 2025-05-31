namespace BookStore.SharedKernel.User;

public static class RoleName
{
    public const string Admin = "Admin";
    public const string Staff = "Staff";
    public const string Customer = "Customer";


    public static IReadOnlyList<string> GetAllRoles() => new List<string>
    {
        Admin,
        Staff,
        Customer,
    };
}
